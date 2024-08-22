INSERT INTO BranchOffices (BranchName, BranchAddress, PhoneNumber)
VALUES ('Downtown Office', '123 Main St, City', '555-1234'),
       ('Uptown Office', '456 Elm St, City', '555-5678');
-- Insert into Users
INSERT INTO Users (Username, Password, Email, Role, RegistrationDate, BranchOfficeID)
VALUES ('jobseeker1', 'password1', 'js1@example.com', 'JobSeeker', '2023-08-01', 1),
       ('employer1', 'password2', 'employer1@example.com', 'Employer', '2023-08-02', 2),
       ('jobseeker2', 'password3', 'js2@example.com', 'JobSeeker', '2023-08-03', 1),
       ('employer2', 'password4', 'employer2@example.com', 'Employer', '2023-08-04', 2);
-- Insert into JobSeekers
INSERT INTO JobSeekers (JobSeekerID, FirstName, LastName, PhoneNumber, Address, ProfileSummary, KeySkills, ExpertField, ResumePath, AcademicDetails, ProfessionalDetails, PreferredIndustry, PreferredSpecialization)
VALUES (3, 'John', 'Doe', '555-1111', '789 Pine St, City', 'Experienced software developer', 'C#, SQL, ASP.NET', 'Software Development', '/resumes/johndoe.pdf', 'B.Sc. Computer Science', '5 years at ABC Corp', 'IT', 'Software Development'),
       (4, 'Jane', 'Smith', '555-2222', '101 Maple St, City', 'Marketing expert', 'SEO, SEM, Social Media', 'Digital Marketing', '/resumes/janesmith.pdf', 'MBA Marketing', '3 years at XYZ Corp', 'Marketing', 'Digital Marketing');
-- Insert into Employers
INSERT INTO Employers (EmployerID, CompanyName, ContactPerson, PhoneNumber, CompanyAddress, IndustryType, WebsiteURL)
VALUES (2, 'TechCorp', 'Alice Johnson', '555-3333', '202 Oak St, City', 'IT', '
http://techcorp.com')
,
       (3, 'MarketInc', 'Bob Williams', '555-4444', '303 Birch St, City', 'Marketing', '
http://marketinc.com');
 
-- Insert into Jobs
INSERT INTO Jobs (EmployerID, JobTitle, JobDescription, IndustryType, Specialization, RequiredSkills, ExperienceLevel, Location, SalaryRange, PostedDate, ApplicationDeadline, JobType)
VALUES (2, 'Software Developer', 'Develop and maintain software applications', 'IT', 'Software Development', 'C#, SQL, ASP.NET', '2-5 years', 'Remote', '$60,000 - $80,000', '2023-08-05', '2023-09-01', 'Full-time'),
       (3, 'Digital Marketing Specialist', 'Manage digital marketing campaigns', 'Marketing', 'Digital Marketing', 'SEO, SEM, Social Media', '1-3 years', 'City Office', '$50,000 - $70,000', '2023-08-06', '2023-09-10', 'Full-time');
-- Insert into Applications
INSERT INTO Applications (JobID, JobSeekerID, ApplicationDate, Status)
VALUES (2, 3, '2023-08-07', 'Pending');
