create table olc_employee (
    empid				int						not null,
    ptel				varchar(40)             null, -- privat telefon
    addusrid            varchar(12)				not null,
    adddate             datetime                null,
    constraint pk_olc_employee primary key (empid),
    constraint fk_olc_employee_empid foreign key (empid) references ols_employee (empid),
    constraint fk_olc_employee_addusrid foreign key (addusrid) references cfw_user (usrid)
)