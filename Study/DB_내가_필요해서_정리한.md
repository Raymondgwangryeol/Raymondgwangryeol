정보처리기사 DBMS 부분을 공부하고 정리한 문서임.

## 트랜잭션(Transaction)
인가받지 않은 사용자로부터 데이터를 보장하기 위해, DBMS가 가져야 하는 특성, 하나의 논리적 기능을 정상적으로 수행하기 위한 ==작업의 기본 단위==
-> 얘 함수같은 놈임.

### 트랜잭션 특성
- 원자성(Atomicity): 트랜잭션 연산 다 성공 아님 실패 택 1 해야함. (부분만 결과 다르면 안 됨)
- 일관성(Consistency): 트랜잭션 수행 전 후의 상태 같아야 함
- 격리성(Isolation): 동시 실행되는 트랜잭션은 서로 독립적이어야 함
- 영속성(Durabliity): 성공 완료된 트랜잭션의 결과는 영속적으로 DB에 저장되어야 함


### 트랜잭션 상태

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/b0b72adf-3880-49ec-9754-a2cb9a8cf0ef)

- 활동(Active): 트랜잭션 실행 중
- 실패(Failed): 실행에 오류 발생. 중단!
- 철회(Aborted): 트랜잭션 비 정상적으로 종료, Rollback 연산 수행
- 부분 완료(Partially Committed): 트랜잭션 마지만 연산까지 수행하고, commit 기다리는 중(커밋 직전의 상태)
- 완료(Committed): 트랜잭션 성공적 종료! Commit 연산 실행 후의 상태

### 트랜잭션 제어어(TCL, Transaction Control Language)
트랜잭션의 결과를 허용하거나 취소하는 목적으로 사용됨.

- COMMIT: 트랜잭션 메모리에 **영구 저장**
- ROLLBACK: 트랜잭션 내용의 **저장 무효화**(되돌리기)
- CHECKPOINT(SAVEPOINT): **ROLLBACK 할 시점** 지정


## 데이터 정의어(DDL: Data Definition Language)
DB구축, 수정 목적으로 사용하는 언어

### DDL 대상
- 도메인(Domain): 하나의 속성이 가질 수 있는 원자값들의 집합(struct 느낌임)
- 스키마(Schema): DB의 구조, 제약조건 등의 정보를 담고 있는 기본 구조
    - 외부 스키마, 개념 스키마, 내부 스키마
- 테이블(Table): 데이터 저장 공간
- 뷰(View): 하나 이상의 물리 테이블에서 유도되는 가상의 테이블
- 인덱스(Index): 검색을 빠르게 하기 위한 데이터 구조
  -인덱스 종류
    - 순서 인덱스(Ordered Index): 데이터가 정렬된 순서로 생성
    - 해시 인덱스(Hash Index): 해시 함수에 의해 키 값으로 직접 데이터에 접근
    - 비트맵 인덱스(Bitmap Index): bit값 0 or 1로 변환, 인덱스로 사용
    - 함수기반 인덱스(Functional Index): 수식이나 함수를 이용해 구성
    - 결합 인덱스(Concatenated Index): 두 개 이상의 컬럼으로 구성
    - 클러스터드 인덱스(Cluster Index): 인덱스 키의 순서에 따라 데이터도 같이 정렬됨. (검색 빠름)
    - 넌클러스터드 인덱스(Non-Clustered Index): 인덱스의 키 값이랑 데이터의 순서랑 아무 상관 없음. ㄹㅇ 저장된 순으로 번호 순서대로 매긴다

  
