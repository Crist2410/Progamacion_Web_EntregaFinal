USE FIT_CENTER;

CREATE TABLE CLASS (
  CLASS_ID INT NOT NULL AUTO_INCREMENT,
  CLASS_NAME VARCHAR(256) NOT NULL,
  CLASS_DESCRIPTION VARCHAR(256),
  START_DATE DATE NOT NULL,
  END_DATE DATE NOT NULL,
  START_TIME TIME NOT NULL,
  END_TIME TIME NOT NULL,
  LOCATION VARCHAR(256) NOT NULL,
  PRIMARY KEY (CLASS_ID)
);

CREATE TABLE USER (
  USER_ID INT NOT NULL AUTO_INCREMENT,
  EMAIL VARCHAR(128) NOT NULL,
  PASSWORD VARCHAR(128) NOT NULL,
  FIRST_NAME VARCHAR(128) NOT NULL,
  LAST_NAME VARCHAR(128) NOT NULL,
  BIRTH_DATE DATE NOT NULL,
  ROLE ENUM('ENTRENADOR', 'ESTUDIANTE') NOT NULL,
  PRIMARY KEY (USER_ID)
);

CREATE TABLE ASSIGNMENT (
  ASSIGNMENT_ID INT NOT NULL AUTO_INCREMENT,
  USER_ID INT NOT NULL,
  CLASS_ID INT NOT NULL,
  ASSIGNMENT_DATE DATE NOT NULL,
  STATUS ENUM('PENDING', 'CONFIRMED', 'CANCELLED') NOT NULL,
  ASSIGNMENT_GRADE INT,
  PRIMARY KEY (ASSIGNMENT_ID),
  FOREIGN KEY (USER_ID) REFERENCES USER(USER_ID),
  FOREIGN KEY (CLASS_ID) REFERENCES CLASS(CLASS_ID)
);