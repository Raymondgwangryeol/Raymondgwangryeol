	📓 공부 자료: ocy7111.log Dev_Oh님 정처기 실기 암기(7. SQL 응용)   
	📓 Procedure 부분 시각 자료 및 공부 자료: https://1d1cblog.tistory.com/109
<br>

정보처리기사 DBMS 부분을 공부하고 정리한 문서임.

# 🍓 트랜잭션(Transaction)
인가받지 않은 사용자로부터 데이터를 보장하기 위해, DBMS가 가져야 하는 특성, 하나의 논리적 기능을 정상적으로 수행하기 위한 <mark>작업의 기본 단위</mark>   
👉 프로시저보다 트랜잭션이 더 넓은 단위

## 🍊 트랜잭션 특성
- 원자성(Atomicity): 트랜잭션 연산 다 성공 아님 실패 택 1 해야함. (부분만 결과 다르면 안 됨)
- 일관성(Consistency): 트랜잭션 수행 전 후의 상태 같아야 함
- 격리성(Isolation): 동시 실행되는 트랜잭션은 서로 독립적이어야 함
- 영속성(Durabliity): 성공 완료된 트랜잭션의 결과는 영속적으로 DB에 저장되어야 함


## 🍊 트랜잭션 상태

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/b0b72adf-3880-49ec-9754-a2cb9a8cf0ef)

- 활동(Active): 트랜잭션 실행 중
- 실패(Failed): 실행에 오류 발생. 중단!
- 철회(Aborted): 트랜잭션 비 정상적으로 종료, Rollback 연산 수행
- 부분 완료(Partially Committed): 트랜잭션 마지만 연산까지 수행하고, commit 기다리는 중(커밋 직전의 상태)
- 완료(Committed): 트랜잭션 성공적 종료! Commit 연산 실행 후의 상태

## 🍊 트랜잭션 제어어(TCL, Transaction Control Language)
트랜잭션의 결과를 허용하거나 취소하는 목적으로 사용됨.

- <code>COMMIT</code>: 트랜잭션 메모리에 **영구 저장**
- <code>ROLLBACK</code>: 트랜잭션 내용의 **저장 무효화**(되돌리기)
- <code>CHECKPOINT</code>(SAVEPOINT): **ROLLBACK 할 시점** 지정

<br><br><br>

# 🍓 데이터 정의어(DDL: Data Definition Language)
DB구축, 수정 목적으로 사용하는 언어
<br><br>



## 🍊 DDL 대상
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
<br><br>

## 🍊 DDL 명령어
<code>CREATE</code>(생성), <code>ALTER</code>(수정), <code>DROP</code>(삭제), <code>TRUNCATE</code>(데이터 싸그리 삭제)
<br><br>

## 🍊 DDL- CREATE
### 1) CREATE DOMAIN
- 도메인(Domain) - 구조체랑 비슷
  - 하나의 속성이 취할 수 있는 동일한 유형의 원자값들의 집합.
  - 특정 속성에서 사용할 데이터의 범위를 사용자가 정의하는 사용자 정의 데이터 타입
```sql
      
CREATE DOMAIN 도메인명 [AS 별명] 데이터 타입
      [DEFAULT 기본값]
       [CONSTRAINT 제약 조건 명 CHECK (범위값)]
```
<br>

### 2) CREATE SCHEMA
- 스키마
    -데이터베이스의 구조, 제약조건에 관한 전반적인 명세(Specification)를 기술한 것.

```sql
 CREATE SCHEMA 스키마 이름 AUTHORIZATION 사용자_ID;
```
<br>

**✏️ [예제]**  
- 소유권자의 사용자 ID가 '홍길동'인 스키마 '대학교'를 정의하는 SQL문
```sql
CREATE SCHEMA 대학교 AUTHORIZATION 홍길동;
```
<br>
### 3) CREATE TABLE
- 테이블(Table)
    - DB의 설계 단계에서는 릴레이션(Relation), 조작, 검색할 때는 테이블(Table)이라고 함.(헉.. 처음 앎..)
    - 그러나 대부분은 테이블과 릴레이션을 구분 없이 사용함.

    ```sql
    CREATE TABLE 릴레이션 이름
    (속성명 데이터_타입 [DEFAULT 기본값] [NOT NULL], ...
     [, PRIMARY KEY(기본키_속성명, ...)] /*PRIMARY KEY: 기본키 속성 지정*/
     [, UNIQUE(대체키_속성명, ...)] /*UNIQUE: 대체키 속성 지정. 중복 값 가질 수 없음*/
     [, FOREIGN KEY(외래키_속성명, ...)] /*FOREIGN KEY ~ REFERENCES ~: 외래키 속성 지정*/
                [REFERENCES 참조 테이블(기본키_속성명, ...)]
                [ON DELETE 옵션] /*참조 테이블 튜플 삭제 시, 기본 테이블에 취해야 할 사항 지정*/
                [ON UPDATE 옵션] /*참조 테이블의 참조 속성 값 변경시, 기본 테이블이 취해야 할 사항 지정*/
     [, CONSTRAINT 제약조건명] [CHECK(조건식)]  /*CONSTRAINT: 제약 조건의 이름을 지정, 또는 제약 사항을 지정*/
                                              /*CHECK: 속성 값에 대한 제약 조건을 정의*/ 
    );
    ```
    
 >- <code>**CASCADE**</code>: 제거할 요소를 참조하는 다른 모든 개체를 함께 제거
 >- RESTRICT: 다른 개체가 제거할 요소를 참조중일 때는 해당 개체를 제거하지 않음

 ![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/cf7a5299-9920-4de1-a929-db2c350a349f)

```sql
 CREATE TABLE 학생
 (이름 VARCHAR(15) NOT NULL,
 학번 CHAR(8),
 전공 CHAR(5),
 성별 SEX,
 생년월일 DATE,
 PRIMARY KEY(학번),
 FOREIGN KEY(전공) REFERENCES 학과(학과코드) /*틀린 부분 = 학과(학과코드)*/
         ON DELETE SET NULL /*틀린 부분 = SET NULL*/
         ON UPDATE CASCADE
 CONSTRAINT 생년월일제약
         CHECK(생년월일 >= '1980-01-01)); /*틀린 부분 = '1980-01-01'*/
```

**✏️ [예제2]**   
- 내가 예시로 짠 SQL. CONSTRAINIT로 PRIMARY KEY를 2개의 속성으로 묶어서 사용함.
```sql
/*Postgres 사용*/
create table Staff
(
	Name_ VARCHAR(5) not null,
	Mail VARCHAR(20),
	Phone VARCHAR(11),
	Dept VARCHAR(10) not null,
	Grade VARCHAR(5),
	Login BOOL not null default false,
	constraint Staff_PK primary key(Name_, Dept)

);
```
### 4) CREATE VIEW
- 뷰(View)
  - 하나 이상의 기본 테이블로부터 유도되는 이름이 있는 가상 테이블(Virtual Table)
  - 일반 테이블롸 달리 뷰는 물리적으로 구현되지 않아 실제로 데이터가 저장이 되지 않는다(아.. 진작 사용할 걸)
  - 뷰를 생성하면 뷰 저의가 시스템 내에 저장되었다가, SQL 내에서 뷰 이름을 사용하면 런타임동안 뷰 정의가 대체되어 수행된다.

```sql
CREATE VIEW 뷰 이름[(속성명[, 속성명, ...])]
AS SELECT문
```

**+) AS가 뭐예요**    
1. 별칭(with SELECT 문)
2. Table이나 속성 명을 다시 새롭게(?) 지정할 때. 다른 이름으로 저장 느낌임 -> SET이 이런 느낌이 좀 더 강하게 남
   
**✏️ [예제]**     
- 고객 테이블에서 '주소'가 '안산시'인 고객들의 '성명'과 '전화번호'를 '안산고객'이라는 뷰로 정의하시오.

```sql
CREATE VIEW 안산고객(성명, 전화번호)
AS SELECT 성명, 전화번호
FROM 고객
WHERE 주소 = '안산시';
```
<br><br>

### 5) CREATE INDEX
- 인덱스(Index): 검색 시간을 단축 시키기 위해 만든 보조적인 데이터 구조

```sql
CREATE [UNIQUE] INDEX 인덱스명 /*UNIQUE: 사용되면 중복 값 없는 속성으로 인덱스를 생성
                                        사용 안 하면 중복 값을 허용해 인덱스 생성*/
ON 테이블명(속성명 [ASC | DESC] [, 속성명 [ASC | DESC]]...) /*Default는 오름차순*/
[CLUSTER] /*사용시 인덱스가 클러스터드 인덱스로 설정됨.*/
```

다시 한 번 알아보는 클러스터드/ 넌 클러스터드 인덱스
- 클러스터드 인덱스(Clustered Index)
  - 인덱스 key의 순서에 맞춰 데이터도 같이 정렬되어 저장됨.
  - 실제로 데이터가 인덱스 따라 순서대로 저장되어 있어서, 굳이 인덱스 겁색 안해도 데이터 빠르게 찾기 가능.
  - 하지만, 데이터 삽입, 삭제시 순서 유지를 위해 데이터를 다시 정렬해야 함.
- 넌 클러스터드 인덱스(Non Clustered Index)
  - 인덱스의 키 값은 순서대로 정렬돼서 데이터에 붙는데, 데이터 자체가 정렬이 되지는 않는 방식
  - 원하는 데이터 검색하려면, 먼저 인덱스를 찾아서 검색해야 함. 클러스터드 인덱스보다 속도가 떨어짐.

**✏️ [예제]**   
- 고객 테이블에서 unique한 특성을 갖고 있는 '고객번호'속성에 대해 내림차순을 정렬하여 '고객번호_idx'라는 이름으로 인덱스를 정의하시오.
```sql
CREATE UNIQUE INDEX 고객번호_idx
ON 고객(고객번호 DESC);
```
<br><br>

## 🍊 DDL- ALTER(테이블의 정의와!!!속성!!! 변경)
### 1) ALTER TABLE
```sql
ALTER TABLE 테이블명 ADD 속성명 데이터_타입 [DEFAULT '기본값']; /*ADD: 새로운 속성(열)을 추가한다*/
ALTER TABLE 테이블명 ALTER 속성명 [SET DEFAULT '기본값']; /*ALTER: 특정 속성의 Default 값을 변경한다*/
ALTER TABLE 테이블명 DROP COLUMN 속성명 [CASCADE]; /*DROP COLUMN: 특정 속성을 삭제한다*/
```

**✏️ [예제1]**   
- 학생 테이블에 최대 3 문자로 구성되는 학년 속성을 추가하시오.

```sql
ALTER TABLE 학생 ADD 학년 VARCHAR(3);
```

**✏️ [예제2]**   
- 학생 테이블의 학번 필드의 데이터 타입과 크기를 VARCHAR(10)으로 하고, NULL값이 입력되지 않도록 변경하시오.
```sql
ALTER TABLE 학생 ALTER 학번 VARCHAR(10), NOT NULL;
```
<br><br>

## 🍊 DDL - DROP
- Table, 인덱스, 스키마, 도메인 등 뭔가 좀 형식적 또는 구조적인 걸 통으로 버리는 명령

```sql
DROP SCHEMA 스키마명 [CASCADE | RESTRICT];
DROP DOMAIN 도메인명 [CASCADE | RESTRICT];
DROP TABLE 테이블명 [CASCADE | RESTRICT];
DROP VIEW 뷰명 [CASCADE | RESTRICT];
DROP INDEX 인덱스명 [CASCADE | RESTRICT];
DROP CONSTRAINT 제약조건명;
```
<br>

#### **✏️[예제]**
- 학생 테이블을 제거하되 학생 테이블을 참조하는 모든 데이터를 함께 제거하시오.
```sql
DROP TABLE 학생 CASCADE;
```
<br><br><br>

# 🍓 데이터 조작어(DML: Data Manipulation Language)
저장된 데이터를 실질적으로 관리하는데 사용하는 명령어
<br>

## 🍊 DML 명령어들
- SELECT(조회), INSERT(삽입), UPDATE(수정), DELETE(삭제)

|명령문|기능|
|:---:|:---|
|SELECT|**속성에 따라** 튜플을 검색|
|INSERT|튜플 삽입|
|DELETE|튜플 삭제|
|UPDATE|튜플 갱신|

<br><br>

## 🍊 DML - SELECT(1)
:boom: **SELECT 순서 - S,F,W,G,H,O**   

```sql
SELECT [PREDICATE] [[테이블명.]속성명 [AS 별칭]] [,[테이블명.]속성명, ...] /*PREDICATE: 검색할 튜플 수 제한하는 명령어들(TOP이라던가..)*/
[,그룹함수(속성명) [AS 별칭]]									/* -DISTINCT: 중복된 튜플이 있으면, 그 중 첫번째만 표시함.*/
									/*속성명: 검색할 속성명, 또는 속성을 이용한 수식을 지정*/
									/*AS: 속성이나 연산의 이름을 다른이름으로 표시하기 위함*/
FROM 테이블명[, 테이블명, ...] /*가져올 테이블 지정*/
[WHERE 조건] /*WHERE: 검색 조건 기술*/
[GROUP BY 속성명, 속성명, ...]
[HAVING 조건]
[ORDER BY 속성명 [ASC | DESC]]; /*오름차순 OR 내림차순 정렬해서 검색할 때 사용. 디폴트는 오름차순*/
```
<br>

### 🍋 조건 연산자
#### 1. 비교 연산자
|연산자|=|<>|>|<|>=|<=|
|:---:|:---:|:---:|:---:|:---:|:---:|:---:|
|**의미**|같다|같지 않다|크다|작다|크거나 같다|작거나 같다|

#### 2. 논리 연산자
- NOT, AND, OR

#### 3. LIKE 연산자
|연산자|%|_|#|
|:---:|:---:|:---:|:---:|
|**의미**|모든 문자를 대표함|문자 하나를 대표함|숫자 하나를 대표함|

#### ✏️[예제1. 기본 검색]
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/22c88705-4928-4104-b2d6-8ff65b1c7a53)
- <사원> 테이블에서 '주소'만 검색하되, 같은 '주소'는 한 번만 출력
  ```sql
  SELECT DISTINCT 주소
  FROM 사원; 
  ```

#### ✏️ [예제2. 기본 검색]
- <사원> 테이블에서 '기본급'에 특별수당 10을 더한 월급을 "XX부서의 XXX의 월급 XXX"형태로 출력
  ```sql
  SELECT 부서 + "부서의" AS 부서 2, 이름 + "의 월급" AS 이름 2, 기본급+10 AS 기본급 2
  FROM 사원;
  ```

#### ✏️ [예제3. 조건 지정 검색]
- 조건을 지정하여 검색할 때는 WHERE절을 사용한다.
- <사원> 테이블에서 '기획'부의 모든 튜플을 검색하시오
  ```sql
  SELECT *
  FROM 사원
  WHERE 부서 = '기획';
  ```
#### ✏️ [예제4. 논리 연산자 AND]
- <사원> 테이블에서 '기획'부서에 근무하면서 "대홍동"에 사는 사람의 튜플을 검색하시오
  ```sql
  SELECT *
  FROM 사원
  WHERE 부서 = '기획' AND 주소 = '대홍동'; 
  ```

#### ✏️ [예제5. 논리 연산자 OR]
- <사원> 테이블에서 부서가 '기획'이거나, '인터넷'인 튜플을 구하시오
  ```sql
  SELECT *
  FROM 사원
  WHERE 부서 = '기획' OR 부서 = '인터넷' 
  ```  
#### ✏️ [예제6. LIKE 연산자]
- <사원> 테이블에서 성이 '김'인 사람의 튜플을 구하시오
  ```sql
  SELECT *
  FROM 사원
  WHERE 이름 LIKE "김%"; 
  ```

#### ✏️[예제7. BETWEEN-AND]
- <사원> 테이블에서 생일이 1969-01-01이거나 1973-12-31 사이인 튜플을 검색하시오
  ```sql
  SELECT *
  FROM 사원
  WHERE 생일 BETWEEN "1969-01-01" AND "1973-12-31"; 
  ```

#### ✏️[예제8. WHERE절 - IS NULL/IS NOT NULL]
- <사원> 테이블에서 '주소'가 NULL인 튜플을 검색하시오
- NULL이 아닌 튜플을 검색하고 싶을 때는 IS NOT NULL을 쓴다.
  ```sql
  SELECT *
  FROM 사원
  WHERE 주소 IS NULL;
  ```

#### ✏️[예제9. 정렬 검색 - ORDER BY]
- <사원> 테이블에서 '주소'를 기준으로 내림차순 정렬시켜 상위 2개 튜플만 검색하시오.
  ```sql
  SELECT TOP 2 *
  FROM 사원
  ORDER BY 주소 DESC;
  ```

#### ✏️[예제10. 중복 정렬]
- <사원> 테이블에서 '부서'를 기준으로 오름차순, 같은 '부서'에 대해서는 '이름'을 기준으로 내림차순 정렬
  ```sql
  SELECT *
  FROM 사원
  ORDER BY 부서 ASC, 이름 DESC; 
  ```

#### ✏️[예제11. 하위 질의]
- 조건절에 주어진 질의를 먼저 수행하여, 그 검색 결과를 조건절의 피연산자로 사용. (주로 2개 이상의 테이블에서 SELECT 할 경우)
- '취미'가 '나이트댄스'인 사원의 '이름'과 '주소'를 검색하시오
  ```sql
  SELECT 이름, 주소
  FROM 사원
  WHERE 취미 = (SELECT 취미 FROM 여가활동 WHERE 취미 = '나이트댄스'); 
  ```

#### ✏️[예제12. 하위 질의-NOT IN]
- 취미활동을 하지 않는 사원들을 구하시오
  ```sql
  SELECT *
  FROM 사원
  WHERE 이름 NOT IN (SELECT 이름 FROM 여가활동); 
  ```

#### ✏️[예제13. 하위 질의-EXISTS]
- 취미활동을 하는 사람들의 부서를 검색하시오
  ```sql
  SELECT 부서
  FROM 사원
  WHERE 이름 EXISTS (SELECT 이름 FROM 여가활동 WHERE 여가활동.이름 = 사원.이름); 
  ```

#### ✏️[예제14. 복수 테이블 검색]
- '경력'이 10년 이상인 사원의 '이름', '부서', '취미', '경력'을 검색하시오
  ```sql
  SELECT 사원.이름, 사원.부서, 여가활동.취미, 여가활동.경력
  FROM 사원, 여가활동
  WHERE 여가활동.경력>=10 AND 여가활동.이름 = 사원.이름);  /*이미 두 개의 테이블을 SELECT 했기 때문인 것 같음.*/
  ```
<br><br>  

## 🍊 DML - SELECT(2): 그룹 함수
```sql
SELECT [PREDICATE] [[테이블명.]속성명 [AS 별칭]] [,[테이블명.]속성명, ...]
[,그룹함수(속성명) [AS 별칭]] /*그룹 함수: GROUP BY 절에 지정된 그룹별로 속성의 값을 집계할 함수를 기술함*/
[, WINDOW 함수 OVER (PARTITION BY 속성명1, 속성명2, ...) /*GROUP BY절을 이용하지 않고, 속성의 값을 집계할 함수를 기술함*/
	       ORDER BY 속성명3, 속성명4, ...] 	       /*  - PARTITION BY: WINDOW 함수의 적용 범위가 될 속성들 지정*/
						       /*  - ORDER BY: PARTITION안에서 사용하는 정렬 기준*/
FROM 테이블명[, 테이블명, ...] 
[WHERE 조건] 
[GROUP BY 속성명, 속성명, ...] /*GROUP BY: 특정 속성을 기준으로 그룹화하여 검색할 때 사용.*/
			      /*          일반적으로 그룹 함수와 함께 사용*/
[HAVING 조건]  /*GROUP BY와 함께 사용, 그룹에 대한 조건을 지정.*/
[ORDER BY 속성명 [ASC | DESC]]; 
```

### 🍋 그룹 함수들
- GROUP BY절에 지정된 그룹별로 속성의 값을 집계할 때 사용함.

|함수|기능|
|:---:|:---|
|COUNT(속성명)|그룹 별 **튜플 수**를 구하는 함수|
|SUM(속성명)|그룹 별 **합계**를 구하는 함수|
|AVG(속성명)|그룹 별 **평균**을 구하는 함수|
|MAX(속성명)|그룹 별 **최대값**을 구하는 함수|
|MIN(속성명)|그룹 별 **최소값**을 구하는 함수|
|STDDEV(속성명)|그룹 별 **표준편차**를 구하는 함수|
|VARIANCE(속성명)|그룹 별 **분산**을 구하는 함수|
|ROLLUP(속성명1, 속성명2, ...)|인수로 주어진 속성을 대상으로 그룹별 소계를 구하는 함수. 속성의 개수가 N개이면 N+1레벨까지, 하위 레벨에서 상위 레벨 순으로 데이터가 집계됨.|
|CUBE(속성명1, 속성명2, ...)|ROLLUP과 유사한 형태이지만, CUBE는 인수로 주어진 속성을 대상으로 **모든 조합의 그룹별 소계**를 구함. 속성의 개수가 n개이면, 2ⁿ레벨까지, 상위 레벨에서 하위 레벨 순으로 데이터가 집계됨.|



#### ✏️[예제 1. GROUP BY - AVG()]

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/57fca3db-1a3e-44fe-9224-9dbca8dc732f)
- <상여금>테이블에서 '부서'별 '상여금'평균을 구하시오
 
```sql
SELECT 부서, AVG(상여금) AS 평균
FROM 상여금
GROUP BY 부서;
```
[결과]   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/25da9b3c-d62c-4cd3-8f34-41a4fefcb981)


#### ✏️[예제 2. GROUP BY - COUNT()]
- <상여금>테이블에서 '부서'별 튜플 수를 구하시오
 
```sql
SELECT 부서, COUNT(*) AS 사원수
FROM 상여금
GROUP BY 부서;
```
[결과]   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/721df575-3a73-4938-a2ff-92349afd295e)


#### ✏️[예제 3. GROUP BY - HAVING]
- <상여금>테이블에서 '상여금'이 100이상인 사원이 2명 이상인 '부서'의 튜플 수를 구하시오
 
```sql
SELECT 부서, COUNT(*) AS 사원수
FROM 상여금
WHERE 상여금>=100
GROUP BY 부서
HAVING COUNT(*)>=2;
```
[결과]   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/e0d44105-4be0-4518-8702-98ec82b8b999)

<br>

### 🍋 GROUP BY - 그룹 지정 검색(ROLLUP, CUBE)
**GROUP BY ROLLUP(칼럼1, 칼럼2...):** 전체 합계는 맨 아래 나오고 -> 2레벨 컬럼 별 합계가 그 위 구역에 나오고 -> 맨 처음으로 3레벨 , 2레벨 별 합계가 나옴   
**GROUP BY CUBE(칼럼1, 칼렴2 ...):** ROLLUP의 반대. 맨 위에 전체 합계 -> 2레벨 컬럼 별 합계 -> 맨 마지막 3레벨, 2레벨 별 합계   
잘 모르시겠다고요? 예제를 보면 그나마 낫습니다!

#### :pencil2: [예제 4. GROUP BY ROLLUP]
- <상여금>테이블의 '부서', '상여내역' 그리고 '상여금'에 대해 부서별 상여내역별 소계와 전체 합계를 검색하시오(속성명은 '상여금합계'로 할 것)
 
```sql
SELECT 부서 상여내역, SUM(상여금) AS "상여금합계"
FROM 상여금;
GROUP BY ROLLUP(부서, 상여내역);
```
[결과]   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/5b410a13-d8bd-488f-af28-49ce4774c48b)


#### ✏️[예제 5. GROUP BY CUBE]
- <상여금>테이블의 '부서', '상여내역' 그리고 '상여금'에 대해 부서별 상여내역별 소계와 전체 합계를 검색하시오(속성명은 '상여금합계'로 할 것)
 
```sql
SELECT 부서 상여내역, SUM(상여금) AS "상여금합계"
FROM 상여금;
GROUP BY CUBE(부서, 상여내역);
```
[결과]   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/76dfc682-c406-47ab-8c8e-1a315473f634)
<br>


### 🍋 Window 함수
- GROUP BY절을 이용하지 않고, 함수의 인수로 지정한 속성의 값을 집계함.
- 이 속성이 집계 범위가 되는데, 이를 윈도우(WINDOW)라고 함.
- WINDOW 함수 종류
  - ROW_NUMBER(): 윈도우 별로(각 범위 당), 각 레코드에 대한 일련 번호 반환.
  - RANK(): 윈도우 별 순위 반환, 공동 순위도 반영.
  - DENSE_RANK(): 윈도우 별 순위 반환 하는데 공동 순위 인정 못 함.

#### ✏️[예제 1. WINDOW ROW_NUMBER()]
- <상여금>테이블에서 '상여내역'별로 '상여금'에 대한 일련 번호를 구하시오(순서는 내림차순, 속성명은 NO로 할 것)
  - 상여내역 별로 = WINDOW
  - 상여금에 대한 일련 변호 = ROW_NUMBER
 
```sql
SELECT 상여내역, 상여금, ROW_NUMBER() OVER (PATITION BY 상여내역 ORDER BY 상여금 DESC) AS NO
FROM 상여금;
```
[결과]   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/ff28f750-ea1e-4949-bd63-292fb7085348)


#### ✏️[예제 2. WINDOW RANK()]
- <상여금>테이블에서 '상여내역'별로 '상여금'에 대한 순위를 구하시오(순서는 내림차순, 속성명은 '상여금순위'로 할 것)
  - 상여내역 별로 = WINDOW
  - 상여금에 대한 순위 = RANK()
 
```sql
SELECT 상여내역, 상여금, RANK() OVER (PATITION BY 상여내역 ORDER BY 상여금 DESC) AS "상여금순위"
FROM 상여금;
```
[결과]   
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/4e912976-3eb6-4d9d-abf7-87777783de1c)


요약하자면 WINDOW는 원하는 조건에 맞게 값을 정돈해서 뽑아내는 바이브?   

GROUP BY는 계산, 통계 등을 보고 싶을 때 하는 바이브?  
<br>

### 🍋 집합 연산자를 이용한 통합 질의
집합 연산자를 사용해, 2개 이상의 테이블의 데이터를 하나로 통합한다!
```sql
SELECT 속성명1, 속성명2, ...
FROM 테이블명
UNION | UNION ALL | INTERSECT | EXCEPT
SELECT 속성명1, 속성명2, ...
FROM 테이블명
[ORDER BY 속성명 [ASC | DESC]];
```
- UNION: 합집합, 중복은 한 번만
- UNION ALL: 합집합, 중복 그냥 묻고 가
- INTERSECT: 교집합
- EXCEPT: 차집합

#### ✏️[예제 1. UNION]
- <사원>테이블과 <직원>테이블을 통합하는 질의문을 작성하시오.(단, 같은 레코드가 중복되어 나오지 않게 하시오)

```sql
SELECT * FROM 사원
UNION
SELECT * FROM 직원;
```
#### ✏️[예제 2. INTERSECT]
- <사원>테이블과 <직원>테이블에 공통으로 존재하는 레코드만 통합하는 질의문을 작성하시오.

```sql
SELECT * FROM 사원
INTERSECT
SELECT * FROM 직원
```

<br><br>

## 🍊 DML-SELECT(3)_JOIN
2개의 릴레이션에서 연관된 튜플들을 결합해서, 하나의 새로운 릴레이션을 반환하는 명령어.
일반적으로 FROM 절에 쓰지만, 릴레이션이 사용되는 곳 어디서나 사용 가능!
크게 INNER JOIN과 OUTER JOIN으로 나뉨.

### 🍋 INNER JOIN
- 일반적으로 EQUI JOIN과 NON-EQUI JOIN으로 구분됨.
- 조건이 없는 INNER JOIN은 CROSS JOIN이랑 똑같은 결과를 반환.
  -CROSS JOIN(교차 조인)?
  	- 조인하는 두 테이블에 있는 튜플들의 순서쌍을 반환(싹 다 뽑아버리기)
  	- 교차 조인의 결과로 반환되는 테이블 행 수는 두 테이블 행 수의 곱과 같음!
<br>

### 🍋 INNER JOIN - EQUI JOIN
- JOIN 조건절에 =를 쓴 JOIN문을 말함.
- 같은 값을 가지는 행을 연결하여 결과를 생성하는 방법.
- = 비교시, 양쪽 다 훑기 때문에 동일한 놈이 2번 나오게 됨. 이 중복되는 친구들 중 한 명을 없애버리는 방법을 NATURAL JOIN이라고 함

<br>

**[WHERE절 이용한 EQUI JOIN]**
```sql
SELECT [테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1, 테이블명2 ...
WHERE 테이블명1.속성명 = 테이블명2. 속성명;
```


**[NATURAL JOIN절을 이용한 EQUI JOIN]**
```sql
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1 NATURAL JOIN 테이블명2 
```


**[JOIN ~USING절을 이용한 EQUI JOIN]**
```sql
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1 JOIN 테이블명2 USING(속성명);
```
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/c43aa03f-77cb-4f2b-a3b0-611cf5bf02f8)

#### ✏️[예제 1. EQUI JOIN]
- <학생>테이블과 <학과> 테이블에서 '학과코드'값이 같은 튜플을 JOIN하여 '학번', '이름', '학과코드', '학과명'을 출력하는 SQL문을 작성하시오.
```sql
/*[방법 1]*/
SELECT 학번, 이름, 학생.학과코드, 학과명
FROM 학생, 학과
WHERE 학과.학생코드 = 학생.학과코드
/*[방법 2]*/
SELECT 학번, 이름, 학생.학과코트, 학과명
FROM 학생 NATURAL JOIN 학과
/*[방법 3]*/
SELECT 학번, 이름, 학생.학과코드 학과명
FROM 학생 JOIN 학과 USING(학과코드); /*학생에 학과를 JOIN해서 학생의 학과코드를 쓰나 봄!*/
```

### INNER JOIN - NON-EQUI JOIN
- JOIN 조건에 =조건 아닌 나머지 비교 연산자(>, <, <>, >=, <=)사용하는 JOIN 방법
```sql
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1, 테이블명2, ...
WHERE (NON-EQUI JOIN 조건);
```


#### ✏️[예제 1. NON-EQUI JOIN]
- <학생>테이블과 <성적등급> 테이블을 JOIN하여 각 학생의 '학번', '이름', '성적', '등급'을 출력하는 SQL문을 작성하시오.
```sql
SELECT 학번, 이름, 성적, 등급
FROM 학생, 성적등급
WHERE (NON-EQUI JOIN 학생.성적 BETWEEN 성적등급.최저 AND 성적등급.최고); /*자꾸 최저.성적등급<=성적 AND 성적<=최고.성적등급 이런식으로 쓰네..*/
```
<br>

### 🍋 OUTER JOIN
- JOIN 조건에 만족하지 않는 튜플들도 싹 다 결과로 출력. 쓰읍 그냥 합집합 아니냐..?
- LEFT OUTER JOIN, RIGHT OUTER JOIN, FULL OUTER JOIN이 있음.
<br>

### 🍋 OUTER JOIN - LEFT OUTER JOIN
- INNER JOIN후, 우측 항 릴레이션의 어떤 튜플과도 안 맞는 좌측 항 릴레이션 튜플들에 NULL값을 붙여 INNER JOIN 결과에 추가.(우측항을 기준으로 왼쪽 거를 갖다 붙임)
```sql
/*방법 1*/
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1 LEFT OUTER JOIN 테이블명2
ON 테이블명1.속성명 = 테이블명2.속성명;

/*방법 2*/
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1, 테이블명2
WHERE 테이블명1.속성명 = 테이블명2.속성명(+);
```
<br>

### 🍋 OUTER JOIN - RIGHT OUTER JOIN
- INNER JOIN후, 좌측 항 릴레이션의 어떤 튜플과도 맍지 않는 우측 항 릴레이션의 튜플들에 NULL값을 붙여 INNER JOIN 결과에 추가.(좌측항을 기준으로 오른쪽 거를 갖다 붙임)
```sql
/*방법 1*/
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1 RIGHT OUTER JOIN 테이블명2
ON 테이블명1.속성명 = 테이블명2.속성명;

/*방법 2* - 이건 그냥 INNER JOIN이잖아../
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1, 테이블명2
WHERE 테이블명1.속성명 = 테이블명2.속성명;
```
<br>

### 🍋 OUTER JOIN - FULL OUTER JOIN
- RIGHT INNER JOIN + LEFT INNER JOIN
- INNER JOIN 후, JOIN 안 된 튜플들 싹 가져와서 NULL값으로 처리해 붙여버리기
```sql
SELECT[테이블명1.]속성명, [테이블명2.]속성명, ...
FROM 테이블명1 FULL OUTER JOIN 테이블명2
ON 테이블명1.속성명 = 테이블명2.속성명;
```
<br>

#### ✏️[예제 1. OUTER JOIN]
- <학생>테이블과 <학과> 테이블에서 '학과코드'값이 같은 튜플을 JOIN하여 '학번', '이름', '학과코드', '학과명'을 검색하는 SQL문 작성하기.
- 이때, '학과코드'가 입력되지 않은 학생도 출력하시오
```sql
/*방법 1*/
SELECT 학번, 이름, 학과코드, 학과명
FROM 학생 LEFT OUTER JOIN 학과
ON 학생.학과코드 = 학과.학과코드;

/*방법 2*/
SELECT 학번, 이름, 학과코드, 학과명
FROM 학생, 학과
WHERE 학생.학과코드 = 학과.학과코드(+)
```
- JOIN 구문을 기준으로 테이블의 위치를 교환하여 RIGHT JOIN을 해도 같은 결과가 나옴.
<br><br>

## 🍊 DML - INSERT
INSERT는 INTO랑 같이 써요
```sql
INSERT INTO 테이블명([속성명1, 속성명2, ...])
VALUES (데이터1, 데이터2, ...);
```   

**[잠깐!]**   
- 각 속성에 대응하는 데이터는 개수와 데이터 유형이 일치해야 함.
- 기본 테이블에 모든 속성을 사용할 때는 속성명 생략 가능
- SELECT 문을 사용해서 다른 테이블의 검색 결과를 삽입할 수 있음.

#### ✏️[예제 1. INSERT]
- <사원>테이블에 (이름 - 홍승현, 부서 - 인터넷)을 삽입하시오.
```sql
INSERT INTO 사원(이름, 부서) VALUES ('홍승현', '인터넷');
```

#### ✏️[예제 2. INSERT]
- <사원>테이블에 ('장보고', '기획', 1993-03-04', '홍제동', 90)를 삽입하시오.
```sql
INSERT INTO 사원 VALUES ('장보고', '기획', '1993-03-04', '홍제동', 90);
```

#### ✏️[예제 3. INSERT] < 다시 봐라
- <사원>테이블에 있는 편집부의 모든 튜플을 편집부원(이름, 생일, 주소, 기본급) 테이블에 삽입하시오.
```sql
INSERT INTO 편집부원(이름, 생일, 주소, 기본급)
SELECT 이름, 생일 주소, 기본급
FROM 사원
WHERE 부서='편집';
```
<br><br>

## 🍊 DML - DELETE
- DELETE는 FROM이랑 같이 써요
```sql
DELETE
FROM 테이블명
[WHERE 조건];
```   
**[잠깐!]**   
- 모든 레코드를 삭제할 때 WHERE절 생략 가능
- 모든 레코드 삭제해도 테이블 구조는 남아있음. 테이블 자체를 날리고 싶다면 DROP

  #### ✏️[예제 1. DELETE]
- <사원>테이블에서 "임꺽정"에대한 튜플을 삭제하시오
  ```sql
  DELETE
  FROM 사원
  WHERE 이름='임꺽정';
  ```
#### ✏️[예제 2. DELETE]
- <사원>테이블에서 '인터넷' 부서에 대한 모든 튜플을 삭제하시오
```sql
DELETE
FROM 사원
WHERE 부서 = '인터넷';
```
#### ✏️[예제 3. DELETE]
- <사원>테이블에 있는 모든 튜플을 삭제하시오
```sql
DELETE
FROM 사원
```
<br><br>

## 🍊 DML - UPDATE
- 기본 테이블의 특정 튜플들의 내용을 변경
- SET이랑 같이 써요
```sql
UPDATE 테이블명
SET 속성명 = 데이터[, 속성명=데이터, ...]
[WHERE 조건];
```
#### ✏️[예제 1. UPDATE]
- <사원>테이블에서 '홍길동'의 주소를 '수색동'으로 수정하기
```sql
UPDATE 사원
SET 주소 = '수색동'
WHERE 이름='홍길동';
```

#### ✏️[예제 2. UPDATE]
- <사원>테이블에서 '황진이'의 부서를 '기획부'로 변경, '기본급'을 5만원 인상하기
```sql
UPDATE 사원
SET 부서 = '기획', 기본급=기본급+5
WHERE 이름 = '황진이';
```
<br><br><br>

# 🍓 데이터 제어어(DCL: Data Control Language)
데이터의 보안, 무결성, 회복, 병행 제어등을 정의하는데 사용.(DBA 데이터 관리 목적)
<br>

|명령|기능|
|:---:|:---|
|COMMIT|명령에 의해 수행된 결과를 실제 디스크에 저장, 데이터베이스 조작 작업이 성공적으로 완료됐음을 관리자에게 알림|
|ROLLBACK|데이터베이스 조작 방법이 비정상적으로 종료됐을 때, 특정 지점의 상태로 복구|
|GRANT|데이터베이스 사용자 권한 부여|
|REVOKE|데이터베이스 사용자 사용 권한 취소|

**🥑 TIP**
- GRANT(그온투)
  - GRANT 권한 ON 테이블 TO 사용자
- REVOKE
  - REVOKE 권한 ON 테이블 FROM 사용자

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/ff87f03d-319a-4149-93b8-e5505dbd6699)
#### ✏️[예제 1. COMMIT ⭕]
- <사원>테이블에서 '사원번호'가 40인 사원의 정보를 삭제한 후, COMMIT을 수정하시오
```sql
DELETE
FROM 사원
WHERE 사원번호 = 40;
COMMIT;
```
- DELETE문 수행 후, COMMIT 명령을 수행했기 때문에, ROLLBACK으로 삭제한 데이터를 다시 되돌릴 수 없다
<br>

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/df952e59-fe04-480c-b3a3-42b3bcfe3af6)

#### ✏️[예제 2. COMMIT ✖️]
- <사원>테이블에서 '사원번호'가 30인 사원의 정보를 삭제하기
```sql
DELETE
FROM 사원
WHERE 사원번호 = 30;
```
- DELETE문 수행 후 COMMIT을 하지 않았으므로, DELETE로 삭제된 레코드는 이후 ROLLBACK으로 다시 되돌릴 수 있다
<br>

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/8a3709a8-b6dc-49de-86c7-57a69537ceac)

#### ✏️[예제 3-1. SAVEPOINT1]
- SAVEPOINT 'S1'을 설정하고, '사원번호'가 20인 사원의 정보 삭제
```sql
SAVEPOINT S1
DELETE FROM 사원 WHERE 사원번호 = 20
```
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/f529276f-b84c-4768-8d3e-2a0105457ba2)

#### ✏️[예제 3-2. SAVEPOINT2]
- SAVEPOINT 'S2'을 설정하고, '사원번호'가 10인 사원의 정보 삭제
```sql
SAVEPOINT S2
DELETE FROM 사원 WHERE 사원번호 =10
```
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/2ddbcf85-b658-4dfb-a80b-50cd33244df1)

#### ✏️[예제 3-3. ROLLBACK]
- SAVEPOINT S1까지 ROLLBACK
```sql
ROLLBACK TO S1;
```
- 예제 3-1 수행 전으로 되돌려짐.
<br>

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/5b902ea9-fefa-4215-8fd1-1b77e85bdd6c)

#### ✏️[예제 3-4. ROLLBACK2]
- 싹 다 ROLLBACK
```sql
ROLLBACK;
```
- DELETE 한 후 COMMIT해버린 부분 빼고 다 ROLLBACK
<br>

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/b18dd7bf-6a86-4c77-acd5-80afcc5eb28c)

<br><br><br>

# 🍓 절차형 SQL(Procedural SQL)
SQL 언어에서도 절차 지향적인 프로그래밍 가능하도록 하는 트랜잭션 언어
<br><br>

## 🍊 Procedure
✔️ 절차형 SQL을 활용해 특정 기능을 수행하는 일종의 트랜잭션 언어.   
✔️ 서버단에 저장되는 SQL 쿼리들을 함수마냥 쓰려고 묶어놓은 것    
✔️ 스템의 일일 마감 작업, 일괄 작업 등에 주로 사용       

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **[구성도]**   
<br>
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/f569e445-6cae-4ebd-b54d-37c20d312a60)


- **DECLARE(필수)**: 선언부. 프로시저의 명칭, 병수, 인수, 테이터 타입을 정의
- **BEGIN/ END(필수)**: 프로시저의 시작과 종료
- **CONTROL**: 조건문 혹은 반복문이 삽입되어 순차 처리됨.
- **SQL**: DML, DCL이 삽입되어 데이터 관리를 위한 조회, 추가, 수정, 삭제 작업을 수행
- **EXCEPTION**: BIGIN ~ END 구문 실행 시 예외 처리할 방법 정의
- **TRANSACTION**: 수행된 데이터 작업들을 DB에 적용할지, 취소할지 결정
<br>

## 🍊 사용자 정의 함수(User-Defined Function)
✔️ SQL 처리 수행후, 수행 결과를 단일 값으로 반환할 수 있는 절차형 SQL   

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  **[구성도]**   
<br>
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/884b4a10-9992-4c7b-8391-53c2393b0ff8)

- **DECLARE(필수)**: 선언부. 사용자 정의 함수의 명칭, 변수 및 상수, 데이터 타입 정의
- **BEGIN/END(필수)**: 사용자 정의 함수의 시작과 종료
- **CONTROL**: 조건문 혹은 반복문이 삽입되어 순차 처리됨.
- **SQL**: SELECT문이 삽입되어 데이터 조회 작업을 수행.
- **EXCEPTION**: BIGIN ~ END 구문 실행 시 예외 처리할 방법 정의
- **RETURN(필수)**: 호출 프로그램에 반환할 값이나 변수를 정의
<br>

## 🍊 Trriger
✔️ 데이터베이스 시스템에서 삽입, 갱신, 삭제 등의 이벤트가 발생할 때마다 관련 작업이 자동으로 수행되는 절차형 SQL   
✔️ 데이터 변경 및 무결성 유지 로그 메시지 출력 등의 목적으로 사용.    
✔️ 트리거에 오류가 있는 경우 트리거가 처리하는 데이터에도 영향을 미침      

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; **[구성도]**   
<br>
![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/21028f08-dcd1-4bcf-a9f4-e9fc976ca0f2)

+ **DECLARE(필수)**: 선언부. 트리거의 명칭, 변수 및 상수, 데이터 타입 정의
+ **EVENT(필수)**: 트리거가 실행되는 조건 명시
+ **BEGIN/END(필수)**: 트리거의 시작과 종료
+ **CONTROL**: 조건문 혹은 반복문이 삽입되어 순차 처리됨.
+ **SQL**: SELECT문이 삽입되어 데이터 조회 작업을 수행.
+ **EXCEPTION**: BIGIN ~ END 구문 실행 시 예외 처리할 방법 정의

