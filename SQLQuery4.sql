ALTER table schedule ADD semester int;
ALTER table schedule add foreign key (semester) REFERENCES Semester(id);