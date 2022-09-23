--drop table olc_cartpay
 
create table olc_cartpay (
	cartpayid int identity    not null,

	payvalue numeric(19,6) null,
	finpaymid varchar(12) not null,
	barcode varchar(100),

	addusrid                  varchar(12)     not null,
	adddate                   datetime        not null,
	delstat                   int             not null,
	constraint pk_olc_cartpay primary key (cartpayid)
)

alter table olc_cartpay add constraint fk_olc_cartpay_addusrid foreign key (addusrid) references cfw_user (usrid) 


