-- delete olc_tmp_sordsord
-- drop table olc_tmp_sordsord
create table olc_tmp_sordsord (
   ssid                 uniqueidentifier     not null,
   sordlineid           int                  not null,
   sordid               int                  not null,
   linenum              int                  not null, 
   itemid               int                  not null,
   itemcode				varchar(25)			 null,
   name01				varchar(100)		 null,
   name02				nvarchar(200)		 null,
   docnum				varchar(12)          not null,
   qty                  numeric(19,6)        null,
   
   reqdate              datetime             not null,
   confqty				numeric(19,6)        null,
   confdeldate			datetime             null,
   ref2                 varchar(30)          null,
   pendingqty			numeric(19,6)        not null,
   fullordqty           numeric(19,6)        not null,
   fullmovqty           numeric(19,6)        not null,
   ordqty               numeric(19,6)        not null,
   movqty               numeric(19,6)        not null,
   selprc               numeric(19,6)        not null,
   seltotprc            numeric(19,6)        null,
   selprctype           int                  null,
   selprcprcid          int                  null,
   discpercnt           numeric(9,4)         not null,
   discpercntprcid      int                  null,
   discval              numeric(19,6)        not null,
   disctotval           numeric(19,6)        null,
   taxid                varchar(12)          not null,
   sordlinestat         int                  not null,
   note                 varchar(200)         null,
   resid                int                  null,
   ucdid                int                  null,
   pjpid                int                  null,
   gen                  int                  not null,
   addusrid             varchar(12)          not null,
   adddate              datetime             not null,
   constraint pk_tmp_sordsord_sordline primary key (sordlineid)
)
go