create table admin
(
	ad_id int identity(1,1) primary key,
	ad_username nvarchar(50) not null unique,
	ad_password nvarchar(50) not null,

)

create table cetagory
(
	
	cat_id int identity(1,1) primary key,
	cat_name nvarchar(50) not null unique,
	cat_iamge nvarchar(max) ,
	cat_fk_ad int foreign key references admin(ad_id),

)

ALTER TABLE cetagory
ADD sts int default 1;


create table users(
	
	u_id int identity(1,1) primary key,
	u_name nvarchar(50) not null ,
	u_email nvarchar(50) not null unique,
	u_phone nvarchar(50) not null,
	u_password nvarchar(50) not null,
	u_image nvarchar(max),

)

create table product
(
	pro_id int identity(1,1) primary key,
	pro_name nvarchar(50) not null,
	pro_iamge nvarchar(max) ,
	pro_price int not null,
	pro_des nvarchar(max),
	pro_fk_cat int foreign key references cetagory(cat_id),
	pro_fk_user int foreign key references users(u_id),
	
)

ALTER TABLE cetagory
ADD sts int default 1;

ALTER TABLE product
ADD bid_price int ;

ALTER TABLE product
ADD aprv int default 0;

ALTER TABLE product
ADD u_id  int FOREIGN KEY REFERENCES users(u_id) ;

ALTER TABLE product
ADD  date_of_post DATETIME default GETDATE();



ALTER TABLE product
DROP COLUMN ;

insert into admin
values('root','1234')

select * from admin
select * from cetagory
select * from product

delete from product