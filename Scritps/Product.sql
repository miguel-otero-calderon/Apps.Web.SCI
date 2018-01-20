create table Product
(	
	CodeProduct varchar(20),
	CodeBar varchar(60),
	CodeIntern varchar(20),
	Description varchar(60),
	DescriptionComplete varchar(200),
	CodeUnitMeasure varchar(4),
	State smallint
)

create table ProductSubFamily(
	CodeProductSubFamily varchar(5),
	CodeProductFamily varchar(5),
	Description varchar(20),
	State smallint	
)