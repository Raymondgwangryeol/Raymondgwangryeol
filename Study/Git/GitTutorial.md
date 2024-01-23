# Git Tutorial

공부 자료: 초보자를 위한 간단한 Git 튜토리얼 (https://nulab.com/ko/learn/software-development/git-tutorial/)
</br></br></br>

## Git의 장점

Git의 버전 제어 시스템을 이용해 소스코드의 수정 내역에 쉽게 접근이 가능하고, 버전이 어떻게 변경되었고 누가 변경했는지 확인 가능!</br> 
전체 기록이 공유저장소에 저장되기 때문에 의도하지 않은 덮어쓰기를 방지할 수 있다.</br>
Git과 같은 버전 제어 시스템을 쓰면 쉽게 할 수 있는 작업들</br>
- 코드 기록 추적
- 협업
- 누가 무엇을 변경했는지 확인
  <img src="https://nulab.com/static/775ba7c8ed89fbce217271b1954b1b2e/5a190/02.png"/>
<img src=""/>
</br></br></br>

## Git의 구성 요소 3가지
<img src="https://nulab.com/static/134fb800f3082b1907b07e5249744360/5a190/01.png"/>

**- 저장소(Repository)**:
</br>
  모든 변경사항을 추적하는 컨테이너. 팀이 만든 모든 커밋을 다 가지고 있다. (git log 명령 쓰면 커밋 내역 확인 가능)
  </br>
  
**- 작업 트리(Work Tree, Work Directory)**
</br>
  작업 중인 파일을 트리로 보여줌. 해당 트리를 보고 수정할 수 있는 파일 시스템으로 볼 수도 있음.</br>
  여기서 트리는 파일의 묶음을 뜻함. 자료구조 그 트리 아님</br>
  
**- 인덱스(Staging Area, Index)**
</br>
  데이터가 commit되기 전에, 아직 처리되지 않는 data가 있는 곳. Staging되면 작업 트리 파일과 저장소 파일을 비교해서 변경된 부분이 있는지 표시함.</br>
</br></br></br>

## 기본 Git Workflow
1. 작업 트리에서 파일 수정
2. 다음에 commit 시킬 변경 내용 준비
3. 변경 사항 commit(commit하면 Staging Area에서 파일을 가져와서, 스냅샷으로 저장소에 저장함.)</br>
&emsp;                 :bookmark_tabs: 스냅샷 : 사진 찍는것 처럼 특정 시점에 스토리지의 파일시스템을 순간포착해서 별도의 이미자나 파일로 저장하는 것.</br>
&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;장애나 data 손상시, 스냅샷 생성 시점으로 복구할 수 있다
<img src="https://nulab.com/static/f4e393df06c82d4fd600c18081507b35/5a190/02.png"/>
</br></br></br>

## Git 파일의 3가지 상태

- 스테이징됨(Staged)
- 커밋됨(Commited)
- 수정됨(Modified)
</br>

**[과정]**
</br>
일단 내가 파일 수정하면 → 파일 트리에서 바로 변경사항 볼 수 있음</br>
→ 이걸 git이 추적할 수 있도록 Stage Area에 올려 놨다가 → 의미 있는 변경사항(Version이라 함) 다음 Commit에 포함(기록)해서 </br>
→ commit해서 파일을 인덱스에서 가져 와 가지고 → 변경 사항이 저장소의 새 스냅샷에 안정하게 기록됨(중앙 공유 저장소에도 변경 내용으로 수정. Modified) </br>
</br>

**:grey_question: commit 전에 add 왜 하나요**
</br>
        Commit은 작업이 완결된 의미있는 변화를 기록하는 것.</br>
        만약 구현한 기능이 3개가 있는데, 이걸 한 번에 커밋하면... 동료 개발자들의 진심어린 덕담을 들을 수 있음...</br>
        이럴 때 add로 원하는 기능을 선택해서 해당 기능만 commit할 수 있음.</br>
</br></br></br>

## 저장소
코드를 저장하기 위해 중앙에 위치한 폴더. 파일과 디렉터리가 포함된 Git 저장소가 있으면, 변경사항 및 버전 추적을 시작할 수 있다.</br>
(일단 먼저 저장할 폴더를 만들라고 한 이유)</br>

</br>

### 로컬 저장소 생성 방법
1. **git init으로 원하는 폴더에서 아예 새로운 저장소를 만들기**
   </br>
   :point_right: version이 저장되지 않은 기존 프로젝트에 git을 도입, 변경사항 추적 시작 가능</br></br>
2. **git clone으로 원격 저장소를 로컬 시스템에 복사하기**
   </br>
   :point_right: 복제된 원격 main branch를 추적하는 local main branch를 자동으로 설정.</br>
   :point_right: 복제된 로컬 저장소에는 원본과 동일한 기록 로그 파일이 있음. 원격 저장소에서 로컬 저장소의 모든 커밋을 참조하고 역추적할 수 있음.</br>
</br></br></br>


## 변경


### 변경 사항 기록
로컬에서 변경 사항이 있다고 다 자동으로 기록되어지는게 아님.</br></br>
작업 트리에서 변경 내용이 생기면</br>
→ 인덱스에서는 수정된 것으로 일단 기록(저장소에 직접 저장되지는 않은 상태) </br>
→ commit을 하면</br>
→ 인덱스에서 staging을 통해 내용 변경할 저장소와 로컬 트리 파일을 비교해 바뀐 부분이 있는지 확인하고(Staged)</br>
→ Stagubg Area에서 파일을 가져와서, 스냅샷으로 저장소에 저장(Commited)</br>
→ 이걸 push하면 원격 저장소에 변경 내용이 적용됨(Modified)
</br></br>

### 변경 사항 커밋
git commit 명령어로 저장소의 Git 기록에 변경 내용을 기록할 수 있음.</br>
커밋한 모든 변경 사항은 각 파일 또는 디렉터리에서 시간순으로 볼 수 있음.(커밋 기록은 로컬 저장소에 저장됨)</br>

각 커밋은 40자 체크섬 해시로 구별되는데, 이 해시값으로 저장소에 저장된 커밋의 상태나 변경사항을 검색할 수 있음.(git log 명령어)</br>

버그 수정, 새로운 기능 추가 등 다양한 유형의 변경사항이 있을 경우, 각 변경 사항 단위로 커밋을 하면(서로 다른 커밋 세트로 분리하면) 변경 사항 내역을 더 쉽게 확인할 수 있다.</br>
  => 이럴 때 work tree를 쓰는 듯?

</br></br></br>

## 변경 취소
새 커밋을 만들 때 git은 스냅샷을 저장하므로, 필요시 이전 버전으로 돌아갈 수 있다.
</br></br>

### 커밋 취소 - git revert
이전 커밋을 취소. 변경 사항을 실행 취소하는 가장 일반적인 방법이다.</br>
커밋을 취소한다고 해서 커밋을 제거하거나, 저장소 수정을 하지 않고, 아예 변경사항을 되돌리는 새로운 커밋을 만들어버림. 기록을 보관하면서 git 저장소의 변경사항을 유지할 수 있음.</br>
git reset이나 git rebase - i를 써서 아예 커밋을 삭제할 수도 있지만, 원격 저장소랑 다른 팀원들의 로컬 저장소랑 구조가 다를 수 있기 때문에, 일반적으로 권장되지 않음.</br></br>
<img src="https://nulab.com/static/d4c010b3dbd1895bf9f63cd5ee63daf9/5a190/01.png"/>
</br></br>

### 커밋 제거 - git reset
HEAD가 이전 커밋을 가리키도록 함. 재설정 모드로 들어가서 재설정 명령의 범위(여기서부터 저기까지 다 지워주세요)를 지정할 수 있음.</br>
HEAD는 현재 commit을 가리키는 포인터.</br>
<img src="https://git-scm.com/book/en/v2/images/reset-checkout.png"/>
</br></br>

### 재설정 모드 - git reset 모드 3가지

1. --soft: 이전 커밋을 취소. HEAD 브랜치가 가리키고 있는 커밋만 변경.(이동)
2. --mixed(기본): HEAD가 가리키고 있는 스냅샷을 인덱스에 업데이트(Index만 변경)
3. --hard: Working directory를 Index의 상태로 업데이트 (Index, Worktree 둘 다 변경)
</br>
<image src="https://nulab.com/static/d3d319a98c48b1ce04570d547a6b0d84/5a190/03.png"/>
</br></br></br>

## 저장소 동기화
다른 팀 구성원들과 변경사항을 공유하려면, 로컬 저장소를 원격 저장소와 자주 동기화해야 함.</br>
Git push, Git pull, Git merge를 사용해 동기화 가능.</br>
</br>

### 변경 사항 푸시 - git push
변경사항을 원격 저장소에 push하면 원격 저장소가 업데이트 되고, 로컬 저장소화 동기화 됨.
</br></br>

### 변경 사항 풀링 - git pulling
원격 저장소에서 로컬로 동기화하는 것. 원격 저장소에서 최신 기록을 다운로드하고, 로컬 저장소로 가져옴.
</br></br>

### 변경 사항 병합 - git merge
일반적으로 많이 사용되는 병합. 커밋 이력을 모두 남길 때 사용함.</br>
최신의 branch를 기존의 branch에 병합시킨다.</br>

<img src = "https://git-scm.com/book/en/v2/images/basic-branching-5.png"/>

**[Merge 방법]**
</br>
(공부 자료: https://hudi.blog/git-merge-squash-rebase/)</br>

  1. **Merge(Fast Forward)**
       </br>
       <a href="https://ibb.co/1JWGR68"><img src="https://i.ibb.co/84ZgPc6/5-B82-D476-5-D8-A-4919-9-E3-E-98595982-F0-DB.png" alt="5-B82-D476-5-D8-A-4919-9-E3-E-98595982-F0-DB" border="0"></a>
       </br>
       병합하려는 branch의 변경 이력을 그대로 main branch에 붙이기</br></br>
  2. **Merge(Recursive)**
       </br>
       <a href="https://ibb.co/31w9RW0"><img src="https://i.ibb.co/F69LHJB/BBC17088-7-F48-44-E4-88-F2-F30650121269.png" alt="BBC17088-7-F48-44-E4-88-F2-F30650121269" border="0"></a>
       </br>
       하나의 branch와 다른 branch의 변경 이력 전체를 합치는 방법.</br>
       변경하고자 하는 branch와 main branch를 공통 부모로 한 새로운 Merge commit 생성.</br></br>
     
  3. **Squash and Marge**
     </br>
     <a href="https://ibb.co/Pc6m6vv"><img src="https://i.ibb.co/T4gvgxx/10-D1-B7-FF-F1-F7-4-A42-9431-36404-E99-DD58.png" alt="10-D1-B7-FF-F1-F7-4-A42-9431-36404-E99-DD58" border="0"></a>
     </br>
       병합할 커밋들을 Squash해서 하나의 새로운 커밋으로 만들고, base branch에 추가</br>
       Squash하면 모든 커밋 이력이 하나로 합쳐지고, 내용이 사라짐.</br></br>
     
  4. **Rebase and Merge**
     </br>
     <a href="https://ibb.co/gyky976"><img src="https://i.ibb.co/6YjYwZN/1-D4-C90-A9-C0-F8-4301-A8-FA-29-E27-C9-DCCE4.png" alt="1-D4-C90-A9-C0-F8-4301-A8-FA-29-E27-C9-DCCE4" border="0"></a>
     </br>
       재설정한 base에 붙이고자 하는 커밋들을 fast foward 병합.</br>
       Rebase를 하면 커밋들의 base가 변경되므로, Commit Hash또한 변경될 수 있음. 때문에 Force Push를 해야할 경우도 발생.</br></br>
</br>

둘 이상의 구성원이 서로 다른 브랜치에서 파일의 동일한 부분을 수정할 경우 병합 도중 **충돌(Conflict)** 발생.</br>
충돌시, 해당 부분을 직접 수정해야 하는데, Git이 수정이 필요한 부분에 **충돌 해결 마커**를 추가함.</br>
</br>

```
<<<<<<< HEAD:index.html
<div id="footer">contact : email.support@github.com</div>
=======
<div id="footer">
 please contact us at support@github.com
</div>
>>>>>>> iss53:index.html
```
</br>

======을 기준으로, 각각의 브랜치에서 수정된 내용을 보여줌.
</br></br></br>

## Git 기록 관리
commit message 변경, 커밋 순서 업데이트, 커밋 스쿼시 등 커밋 기록을 수정하는 방법에 대해 설명.
</br></br>

### 커밋 수정 - git commit --amend동일한 브랜치에서 가장 최근 커밋을 수정할 수 있음.
새 파일이나, 업데이트 된 파일을 이전 커밋에 추가하거나, 이전 커밋의 커밋 메시지를 편집, 추가하는데 사용.
</br></br>

### 새 브랜치에 커밋 복사 - git rebase
A와 B가 동시에 C부분을 개발하는 상황에서, A따로 B따로 커밋하면 너무 복잡해져서 보기가 힘들다.</br>
A,B중 먼저 commit한 거를 새로운 base로 설정(rebase), 다른 한 사람의commit을 base commit에 줄줄이 붙여 합치는 명령어.</br>
<img src="https://git-scm.com/book/en/v2/images/basic-rebase-3.png"/>


rebase 옵션 중 -i(--interactive)를 쓰면,</br>
- 과거 커밋 메시지 다시 쓰기(reword)</br>
- 커밋 그룹을 함께 스쿼시(squash, fixup)</br>
- 커밋 되지 않은 파일 추가(edit)</br>

등을 할 수 있음.</br>
</br>
**git rebase -i {수정할 커밋의 직전 커밋}**
</br>
</br>
위의 명령어를 실행하면, 커밋 순서가 정리되어있는 문서 내용이 vim editor로 뜸.</br>
vim editor에 적혀있는 command들이 있는데, 해당 command를 이용해 특정 커밋의 내용을 수정할 수 있음.</br>
```
noop

# Rebase ... onto ... (1 command)
#
# Commands:
# p, pick <commit> = use commit
# r, reword <commit> = use commit, but edit the commit message
# e, edit <commit> = use commit, but stop for amending
# s, squash <commit> = use commit, but meld into previous commit
# f, fixup [-C | -c] <commit> = like "squash" but keep only the previous
#                    commit's log message, unless -C is used, in which case
#                    keep only this commit's message; -c is same as -C but
#                    opens the editor
# x, exec <command> = run command (the rest of the line) using shell
# b, break = stop here (continue rebase later with 'git rebase --continue')
# d, drop <commit> = remove commit
# l, label <label> = label current HEAD with a name
# t, reset <label> = reset HEAD to a label
# m, merge [-C <commit> | -c <commit>] <label> [# <oneline>]
#         create a merge commit using the original merge commit's
#         message (or the oneline, if no original merge commit was
#         specified); use -c <commit> to reword the commit message
# u, update-ref <ref> = track a placeholder for the <ref> to be updated
#                       to this position in the new commits. The <ref> is
#                       updated at the end of the rebase
#
# These lines can be re-ordered; they are executed from top to bottom.
#
# If you remove a line here THAT COMMIT WILL BE LOST.
#
# However, if you remove everything, the rebase will be aborted.
#

```
</br>

**[명령어들]**
  </br>
    - **pick:** 그냥 쓸게요
    </br>
    - **reword:** 커밋 메시지 수정할게요
    </br>
    - **edit:** 커밋의 명령어 뿐 아니라, 작업내용도 수정 가능
    </br>
    - **squash:** 각 커밋들의 메시지들이 합쳐짐
    </br>
    - **fixup:** 이전의 커밋 메시지만 남김
    </br>
</br></br>

### 다른 브랜치에 커밋 복사 - git cherry-pick
다른 브랜치에서 현재 브랜치로 기존 커밋을 복사하는 명령어.</br>
잘못된 브랜치에서 올바른 브랜치로 커밋 이동, 다른 브랜치의 기존 커밋을 현재 브랜치에 추가할 때 사용
</br></br>

### 커밋 병합 - git merge --squash(Squash and Merge)
여러 커밋을 단일 커밋으로 병합. 해당 옵션을 사용하면 squash할 커밋들이 묶어져 그룹화되며, 그룹화 된 커밋들이 하나의 새로운 커밋이 됨. 그리고 병합.
</br></br>

