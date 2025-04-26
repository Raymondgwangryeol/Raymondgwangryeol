import numpy as np
from typing import Optional

class IDCBoost(object):
    
    def __init__(self, D: np.ndarray, T: np.ndarray, T_conf: np.ndarray, T_last_updates: Optional[np.ndarray]=None, useS: bool=False, useSB: bool=False, useVT: bool=False):
        """
        IDCBoost class를 초기화 합니다.
        
        Args:
            D (np.ndarray): Detection array (shape: [n, 8], 각 detection의 bbox 및 속성)
            T (np.ndarray): Track array (shape: [m, 8], 각 track의 bbox 및 속성)
            T_conf (np.ndarray): Track의 confidence scores (shape: [m])
            T_last_updates (Optional[np.ndarray]): Track의 마지막 업데이트 frame (shape: [m]), VT 사용 시 필요
            useS (bool): Similarity를 사용할지 여부
            useSB (bool): Similarity Boosting을 사용할지 여부
            useVT (bool): Variable Threshold를 사용할지 여부
        """
        self.D = D
        self.T = T
        self.T_conf = T_conf
        self.T_last_updates = None if T_last_updates is None else T_last_updates
        self.useS = useS
        self.useSB = useSB
        self.useVT = useVT
    
    def compute_threshold(self, last_update: int, gamma: float, beta_low: float, beta_high: float) -> float:
        """
        VT를 사용할 때 필요한 threshold를 계산합니다.
        
        Args:
            last_update (int): Track이 마지막으로 업데이트된 프레임 번호
            gamma (float): Threshold 감소 속도
            beta_low (float): 최저 threshold 값
            beta_high (float): 초기 threshold 값
        
        Returns:
            float: 계산된 threshold 값
        """
        return np.maximum(beta_low, beta_high - gamma * (last_update - 1))
    
    def IoU(self, D: np.ndarray, T: np.ndarray) -> float:
        """
        IoU를 계산합니다.
        
        Args:
            D (np.ndarray): Detection bbox (shape: [8])
            T (np.ndarray): Track bbox (shape: [8])
        
        Returns:
            float: IoU 값
        """
        x_d, y_d, w_d, h_d ,_, _, _, _ = D
        x_t, y_t, w_t, h_t ,_, _, _, _ = T

        x1_d, y1_d, x2_d, y2_d = x_d, y_d, x_d + w_d, y_d + h_d
        x1_t, y1_t, x2_t, y2_t = x_t, y_t, x_t + w_t, y_t + h_t

        inter_x1 = max(x1_d, x1_t)
        inter_y1 = max(y1_d, y1_t)
        inter_x2 = min(x2_d, x2_t)
        inter_y2 = min(y2_d, y2_t)

        inter_w = max(0, inter_x2 - inter_x1)
        inter_h = max(0, inter_y2 - inter_y1)
        inter_area = inter_w * inter_h

        D_area = w_d * h_d
        T_area = w_t * h_t

        union_area = D_area + T_area - inter_area

        if union_area == 0:
            return 0.0
        else:
            return inter_area / union_area
    
    def SBIoU(self, D_i: np.ndarray, T_j: np.ndarray, c_tj: float) -> float:
        """
        Similarity 계산 시 사용하는 Scale-based IoU를 계산합니다.
        
        Args:
            D_i (np.ndarray): Detection bbox
            T_j (np.ndarray): Track bbox
            c_tj (float): Track의 confidence
        
        Returns:
            float: SBIoU 값
        """
        scale_D = (1 - c_tj) / 4
        scale_T = (1 - c_tj) / 2

        D_i_scaled = D_i.copy()
        T_j_scaled = T_j.copy()

        D_i_scaled[:4] *= scale_D
        T_j_scaled[:4] *= scale_T

        return self.IoU(D_i_scaled, T_j_scaled)
  
    def Sshape(self, D_i: np.ndarray, T_j: np.ndarray) -> float:
        w_d, h_d = D_i[2], D_i[3]
        w_t, h_t = T_j[2], T_j[3]
        
        numerator = abs(w_d - w_t) + abs(h_d - h_t)
        denominator = max(w_d, w_t)
        
        if denominator == 0:
            return 0.0 
        else:
            return 1-(numerator / denominator)

    def SMhD(self, D_i: np.ndarray, T_j: np.ndarray) -> float:
        """
        Detection과 Track의 속성(dot_x, dot_y, dot_h) 거리 기반 유사도를 계산합니다.
        
        Args:
            D_i (np.ndarray): Detection bbox
            T_j (np.ndarray): Track bbox
        
        Returns:
            float: 속성 유사도
        """

        diff = np.array([D_i[4] - T_j[4], D_i[5] - T_j[5], D_i[3] - T_j[3]])
        d = np.linalg.norm(diff)*0.5
        return np.exp(-d)
    
    def compute_similarity(self, D: np.ndarray, T: np.ndarray) -> np.ndarray:
        """
        Detection과 Track 간 전체 Similarity Matrix를 계산합니다.
        
        Returns:
            np.ndarray: Similarity matrix (shape: [n, m])
        """
        S = np.zeros((len(D), len(T)))
        for i in range(len(D)):
            for j in range(len(T)):
                S[i, j] = (self.SBIoU(D[i], T[j], self.T_conf[j]) + self.SMhD(D[i], T[j]) + self.Sshape(D[i], T[j])) / 3
        return S

    def calculate(self, frame_cnt: int, beta_c: float, tau: float, q: float=1.0, alpha: float=0.5, b_low: float=0.8, b_high: float=0.95):
        """
        Detection confidence를 계산합니다.
        
        Args:
            frame_cnt (int): 전체 프레임 수
            beta_c (float): confidence scaling factor
            tau (float): VT 적용시 강제 confidence threshold
            q (float, optional): SB 적용시 power term (default=1.0)
            alpha (float, optional): SB blending weight (default=0.5)
            b_low (float, optional): VT 최저 threshold 값 (default=0.8)
            b_high (float, optional): VT 최고 threshold 값 (default=0.95)
        
        Returns:
            np.ndarray: 계산된 confidence 배열 (shape: [n])
        """
        n = len(self.D)
        m = len(self.T)
        S = np.empty((n, m))
        c = np.zeros(n)
        gamma = (b_high - b_low) / frame_cnt
        
        if self.useS:
            S = self.compute_similarity(self.D, self.T)
        else:
            for i in range(n):
                for j in range(m):
                    S[i, j] = self.IoU(self.D[i], self.T[j])                  
        
        for i in range(n):
            c[i] = beta_c * np.max(S[i])
        
        if self.useSB:
            for i in range(n):
                boost = alpha * c[i] + (1 - alpha) * (np.max(S[i]) ** q)
                c[i] = max(c[i], boost)
        
        if self.useVT:
            for j in range(m):
                beta_j = self.compute_threshold(self.T_last_updates[j], gamma, b_low, b_high)
                for i in range(n):
                    if S[i, j] >= beta_j:
                        c[i] = max(c[i], tau)
                        break
                    
        return c

# ==================== 실험 코드 ======================

# 실험용 Detection 데이터
D = np.array([
    [10, 10, 20, 20, 0.5, 0.5, 20, 20],
    [30, 30, 15, 15, 0.6, 0.6, 15, 15],
])

# 실험용 Track 데이터
T = np.array([
    [12, 12, 20, 20, 0.52, 0.52, 20, 20],
    [100, 100, 15, 15, 1.0, 1.0, 15, 15],
])

T_conf = np.array([0.9, 0.7])
last_updates = np.array([1, 5])

# 실험
print("\n기본 실험 (S 안씀, SB 안씀, VT 안씀)")
idcboost = IDCBoost(D, T, T_conf)
c = idcboost.calculate(frame_cnt=20, beta_c=0.9, tau=0.5)
print(c)

print("\nS만 사용하는 실험 (useS=True)")
idcboost_s = IDCBoost(D, T, T_conf, last_updates, useS=True)
c_s = idcboost_s.calculate(frame_cnt=20, beta_c=0.9, tau=0.5)
print(c_s)

print("\nSB만 사용하는 실험 (useS=True, useSB=True)")
idcboost_sb = IDCBoost(D, T, T_conf, last_updates, useS=True, useSB=True)
c_sb = idcboost_sb.calculate(frame_cnt=20, beta_c=0.9, tau=0.5)
print(c_sb)

print("\nVT만 사용하는 실험 (useS=True, useVT=True)")
idcboost_vt = IDCBoost(D, T, T_conf, last_updates, useS=True, useVT=True)
c_vt = idcboost_vt.calculate(frame_cnt=20, beta_c=0.9, tau=0.5)
print(c_vt)
