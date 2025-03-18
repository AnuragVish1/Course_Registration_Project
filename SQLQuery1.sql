-- Step 1: Drop the foreign key constraint
ALTER TABLE Courses
DROP CONSTRAINT FK_Courses_Semester;

-- Step 2: Drop the semester_id column
ALTER TABLE Courses
DROP COLUMN semester_id;