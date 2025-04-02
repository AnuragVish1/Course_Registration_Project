ALTER table courses add semester int;
ALTER table courses add Foreign Key (semester) References semester(id);