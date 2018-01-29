DECLARE @SchemaID int = (SELECT schema_id FROM sys.schemas WHERE name = 'X')

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AppointmentType' AND schema_id = @SchemaID)
	BEGIN
		CREATE TABLE AppointmentType
		( 
			id INT NOT NULL PRIMARY KEY,
			name VARCHAR(50) NOT NULL,
		);
		ALTER TABLE Appointment
			ADD IdAppointmentType INTEGER,
			FOREIGN KEY(IdAppointmentType) REFERENCES AppointmentType(id);

		INSERT INTO AppointmentType (Id, Name) VALUES (1, 'Therapeutic');
		INSERT INTO AppointmentType (Id, Name) VALUES (2, 'Surgery');
		INSERT INTO AppointmentType (Id, Name) VALUES (3, 'Vaccine');

	END
GO