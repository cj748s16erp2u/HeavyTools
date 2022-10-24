 --drop table olc_prctable_current
 

create table olc_prctable_current (
  prccid int identity not null, 
  itemid int not null,
  date datetime not null,
  priceGrossHuf numeric(19, 6) null, 
  priceSaleGrossHuf numeric(19, 6) null, 
  retailPriceGrossHuf numeric(19, 6) null, 
  retailPriceSaleGrossHuf numeric(19, 6) null, 
  priceGrossEurEn numeric(19, 6) null, 
  priceSaleGrossEurEn numeric(19, 6) null, 
  retailPriceGrossEurEn numeric(19, 6) null, 
  retailPriceSaleGrossEurEn numeric(19, 6) null, 
  priceGrossEurSK numeric(19, 6) null, 
  priceSaleGrossEurSK numeric(19, 6) null, 
  priceGrossCzkCz numeric(19, 6) null, 
  priceSaleGrossCzkCz numeric(19, 6) null, 
  retailPriceGrossCzkCz numeric(19, 6) null, 
  retailPriceSaleGrossCzkCz numeric(19, 6) null, 
  priceGrossRonRo numeric(19, 6) null, 
  priceSaleGrossRonRo numeric(19, 6) null, 
  retailPriceGrossRonRo numeric(19, 6) null, 
  retailPriceSaleGrossRonRo numeric(19, 6) null, 
  constraint pk_olc_prctable_current primary key (prccid)
) 


alter table olc_prctable_current 
		add constraint fk_olc_prctable_current_itemid 
		       foreign key (itemid) 
			references ols_item (itemid) 

go
