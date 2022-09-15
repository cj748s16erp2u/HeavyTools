--drop table olc_cart

create table olc_cart (
	cartid                  	int identity    not null,

	itemid                  	int null,
	loyaltyCardNo				varchar(100),
	cupon						varchar(100),

	isHandPrice					int,

	orignalSelPrc numeric(19,6) null,
	orignalGrossPrc numeric(19,6) null,
	orignalTotVal numeric(19,6) null,
	selPrc numeric(19,6) null,
	grossPrc numeric(19,6) null,
	netVal numeric(19,6) null,
	taxVal numeric(19,6) null,
	totVal numeric(19,6) null,
	aid	int null,
  
	addusrid                  varchar(12)     not null,
	adddate                   datetime        not null,
	delstat                   int             not null,
	constraint pk_olc_cart primary key (cartid)
)

alter table olc_cart add constraint fk_olc_cart_addusrid foreign key (addusrid) references cfw_user (usrid)
alter table olc_cart add constraint fk_olc_cart_aid foreign key (aid) references olc_action (aid)
alter table olc_cart add constraint fk_olc_cart_itemid foreign key (itemid) references ols_item (itemid)


