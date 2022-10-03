--drop table olc_itemassortment

create table olc_itemassortment (
	isoid int identity    not null,
	assortmentitemid int not null,
	itemid int not null,

	count int not null,

	addusrid                  varchar(12)     not null,
	adddate                   datetime        not null,
	delstat                   int             not null,
	constraint pk_olc_itemassortment primary key (isoid)
)

alter table olc_itemassortment add constraint fk_olc_itemassortment_addusrid foreign key (addusrid) references cfw_user (usrid) 
alter table olc_itemassortment add constraint fk_olc_itemassortment_assortmentitemid foreign key (assortmentitemid) references ols_item (itemid) 
alter table olc_itemassortment add constraint fk_olc_itemassortment_itemid foreign key (itemid) references ols_item (itemid) 



