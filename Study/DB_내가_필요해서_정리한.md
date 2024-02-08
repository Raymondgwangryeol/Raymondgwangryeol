정보처리기사 DBMS 부분을 공부하고 정리한 문서임.

# 트랜잭션(Transaction)
인가받지 않은 사용자로부터 데이터를 보장하기 위해, DBMS가 가져야 하는 특성, 하나의 논리적 기능을 정상적으로 수행하기 위한 ==작업의 기본 단위==
-> 얘 함수같은 놈임.

## 트랜잭션 특성
- 원자성(Atomicity): 트랜잭션 연산 다 성공 아님 실패 택 1 해야함. (부분만 결과 다르면 안 됨)
- 일관성(Consistency): 트랜잭션 수행 전 후의 상태 같아야 함
- 격리성(Isolation): 동시 실행되는 트랜잭션은 서로 독립적이어야 함
- 영속성(Durabliity): 성공 완료된 트랜잭션의 결과는 영속적으로 DB에 저장되어야 함


## 트랜잭션 상태

![image](https://github.com/Raymondgwangryeol/Raymondgwangryeol/assets/32587541/b0b72adf-3880-49ec-9754-a2cb9a8cf0ef)

- 활동(Active): 트랜잭션 실행 중
- 실패(Failed): 실행에 오류 발생. 중단!
- 철회(Aborted): 트랜잭션 비 정상적으로 종료, Rollback 연산 수행
- 부분 완료(Partially Committed): 트랜잭션 마지만 연산까지 수행하고, commit 기다리는 중(커밋 직전의 상태)
- 완료(Committed): 트랜잭션 성공적 종료! Commit 연산 실행 후의 상태

## 트랜잭션 제어어(TCL, Transaction Control Language)
트랜잭션의 결과를 허용하거나 취소하는 목적으로 사용됨.

- <code>COMMIT</code>: 트랜잭션 메모리에 **영구 저장**
- <code>ROLLBACK</code>: 트랜잭션 내용의 **저장 무효화**(되돌리기)
- <code>CHECKPOINT</code>(SAVEPOINT): **ROLLBACK 할 시점** 지정


# 데이터 정의어(DDL: Data Definition Language)
DB구축, 수정 목적으로 사용하는 언어

## DDL 대상
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

## DDL 명령어
<code>CREATE</code>(생성), <code>ALTER</code>(수정), <code>DROP</code>(삭제), <code>TRUNCATE</code>(데이터 싸그리 삭제)

## DDL- CREATE
### 1) CREATE DOMAIN
- 도메인(Domain) - 구조체랑 비슷
  - 하나의 속성이 취할 수 있는 동일한 유형의 원자값들의 집합.
  - 특정 속성에서 사용할 데이터의 범위를 사용자가 정의하는 사용자 정의 데이터 타입
      ```sql
      
      CREATE DOMAIN 도메인명 [AS 별명] 데이터 타입
             [DEFAULT 기본값]
             [CONSTRAINT 제약 조건 명 CHECK (범위값)]
     ```

### 2) CREATE SCHEMA
- 스키마
    -데이터베이스의 구조, 제약조건에 관한 전반적인 명세(Specification)를 기술한 것.

```sql
 CREATE SCHEMA 스키마 이름 AUTHORIZATION 사용자_ID;
```

[예제]
소유권자의 사용자 ID가 '홍길동'인 스키마 '대학교'를 정의하는 SQL문

```sql
CREATE SCHEMA 대학교 AUTHORIZATION 홍길동;
```

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
[예제2]
내가 예시로 짠 SQL. CONSTRAINIT로 PRIMARY KEY를 2개의 속성으로 묶어서 사용함.
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

+)AS가 뭐예요
1. 별칭(with SELECT 문)
2. Table이나 속성 명을 다시 새롭게(?) 지정할 때. 다른 이름으로 저장 느낌임 -> SET이 이런 느낌이 좀 더 강하게 남
   
[예제]
고객 테이블에서 '주소'가 '안산시'인 고객들의 '성명'과 '전화번호'를 '안산고객'이라는 뷰로 정의하시오.

```sql
CREATE VIEW 안산고객(성명, 전화번호)
AS SELECT 성명, 전화번호
FROM 고객
WHERE 주소 = '안산시';
```

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

[예제]
고객 테이블에서 unique한 특성을 갖고 있는 '고객번호'속성에 대해 내림차순을 정렬하여 '고객번호_idx'라는 이름으로 인덱스를 정의하시오.
```sql
CREATE UNIQUE INDEX 고객번호_idx
ON 고객(고객번호 DESC);
```

## DDL- ALTER(테이블의 정의와!!!속성!!! 변경)
### 1) ALTER TABLE
```sql
ALTER TABLE 테이블명 ADD 속성명 데이터_타입 [DEFAULT '기본값']; /*ADD: 새로운 속성(열)을 추가한다*/
ALTER TABLE 테이블명 ALTHER 속성명 [SET DEFAULT '기본값']; /*ALTER: 특정 속성의 Default 값을 변경한다*/
ALTER TABLE 테이블명 DROP COLUMN 속성명 [CASCADE]; /*DROP COLUMN: 특정 속성을 삭제한다*/
```

[예제1]
학생 테이블에 최대 3 문자로 구성되는 학년 속성을 추가하시오.

```sql
ALTER TABLE 학생 ADD 학년 VARCHAR(3);
```

[예제2]
학생 테이블의 학번 필드의 데이터 타입과 크기를 VARCHAR(10)으로 하고, NULL값이 입력되지 않도록 변경하시오.
```sql
ALTER TABLE 학생 ALTER 학번 VARCHAR(10), NOT NULL;
```

## DDL - DROP
- Table, 인덱스, 스키마, 도메인 등 뭔가 좀 형식적 또는 구조적인 걸 통으로 버리는 명령

```sql
DROP SCHEMA 스키마명 [CASCADE | RESTRICT];
DROP DOMAIN 도메인명 [CASCADE | RESTRICT];
DROP TABLE 테이블명 [CASCADE | RESTRICT];
DROP VIEW 뷰명 [CASCADE | RESTRICT];
DROP INDEX 인덱스명 [CASCADE | RESTRICT];
DROP CONSTRAINT 제약조건명;
```

[예제]
학생 테이블을 제거하되 학생 테이블을 참조하는 모든 데이터를 함께 제거하시오.
```sql
DROP TABLE 학생 CASCADE;
```

