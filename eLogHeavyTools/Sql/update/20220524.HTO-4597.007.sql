/*  
	Cikk kiegészítések
*/    
create table olc_item (
	itemid				int, 
	imsid				int,
	isrlid				int,
	colortype1			int,
	colorname			varchar(100),
	colortype2			int,
	colortype3			int,
	materialtype		int,
	patterntype			int,
	patterntype2		int,
	
	addusrid			varchar(12),
	adddate				datetime,
	delstat				int not null,
	constraint pk_olc_item primary key (itemid), 
	constraint fk_olc_item_imsid foreign key (imsid) references olc_itemmodelseason(imsid),
	constraint fk_olc_item_isrlid foreign key (isrlid) references olc_itemsizerangeline(isrlid),
	constraint fk_olc_item_addusrid foreign key (addusrid) references cfw_user(usrid)
)

go 