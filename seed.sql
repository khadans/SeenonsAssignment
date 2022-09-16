\connect seenonsDb

CREATE TABLE Container
(
    id SERIAL PRIMARY KEY,
    type varchar(50) NOT NULL,
    name varchar(50) NOT NULL,
    active boolean NOT NULL
);

CREATE TABLE Size
(
    id SERIAL PRIMARY KEY,
    size integer NOT NULL,
    container_product_id integer,
    discount_percentage integer,
    unit_price_pickup money,
    unit_price_rent money,
    unit_price_placement money,
    CONSTRAINT fk_container
      FOREIGN KEY(container_product_id)
	  REFERENCES Container(id)
);

CREATE TABLE Stream
(
    stream_product_id SERIAL PRIMARY KEY,
    type varchar(30) NOT NULL,
    name varchar(30) NOt NULL,
    active boolean NOT NULL
);

CREATE TABLE LogisticalProvider
(
    id SERIAL PRIMARY KEY,
    name varchar(30) NOT NULL
);

CREATE TABLE PickupArea
(
    id SERIAL PRIMARY KEY,
    PostalCodeFrom integer NOT NULL,
    PostalCodeTo integer NOT NULL
);

CREATE TABLE ProviderPickupArea
(
    id SERIAL PRIMARY KEY,
    PickupAreaId integer NOT NULL,
    LogisticalProviderId integer NOT NULL,
    CONSTRAINT fk_PickupArea
      FOREIGN KEY(PickupAreaId)
	  REFERENCES PickupArea(id),
	CONSTRAINT fk_LogisticalProvider
      FOREIGN KEY(LogisticalProviderId)
	  REFERENCES LogisticalProvider(id)
);

CREATE TABLE ProviderPickupArea_Stream
(
    id SERIAL PRIMARY KEY,
    ProviderPickupAreaId integer NOT NULL,
    StreamId integer NOT NULL,
    CONSTRAINT fk_Stream
      FOREIGN KEY(StreamId)
	  REFERENCES Stream(stream_product_id),
	CONSTRAINT fk_ProviderPickupArea
      FOREIGN KEY(ProviderPickupAreaId)
	  REFERENCES ProviderPickupArea(id)
);

CREATE TABLE ProviderPickupArea_Container
(
    id SERIAL PRIMARY KEY,
    ProviderPickupAreaId integer NOT NULL,
    ContainerId integer NOT NULL,
    CONSTRAINT fk_Container
      FOREIGN KEY(ContainerId)
	  REFERENCES Container(id),
	CONSTRAINT fk_ProviderPickupArea
      FOREIGN KEY(ProviderPickupAreaId)
	  REFERENCES ProviderPickupArea(id)
);

CREATE TABLE ProviderPickupAreaDay
(
    id SERIAL PRIMARY KEY,
    ProviderPickupAreaId integer NOT NULL,
    Day integer NOT NULL,
    WeekRecurrence integer NOT NULL,
	CONSTRAINT fk_ProviderPickupArea
      FOREIGN KEY(ProviderPickupAreaId)
	  REFERENCES ProviderPickupArea(id)
);

CREATE TABLE ProviderPickupAreaDayTimeSlot
(
    id SERIAL PRIMARY KEY,
    ProviderPickupAreaDayId integer NOT NULL,
    PickupStart time NOT NULL,
	CONSTRAINT fk_ProviderPickupAreaDay
      FOREIGN KEY(ProviderPickupAreaDayId)
	  REFERENCES ProviderPickupAreaDay(id)
);

CREATE TABLE StreamSize
(
    id SERIAL PRIMARY KEY,
    StreamId integer NOT NULL,
    SizeId integer NOT NULL,
	CONSTRAINT fk_Stream
      FOREIGN KEY(StreamId)
	  REFERENCES Stream(stream_product_id),
	CONSTRAINT fk_Size
      FOREIGN KEY(SizeId)
	  REFERENCES Size(id)
);

CREATE TABLE Customer
(
    id SERIAL PRIMARY KEY,
    firstname varchar(30) NOT NULL,
    lastname varchar(30) NOT NULL,
    company varchar(30) NOT NULL,
    email varchar(30) NOT NULL,
    mobile varchar(30) NOT NULL,
    postalcode integer,
    street varchar(30) NOT NULL,
    streetnumber varchar(6) NOT NULL,
    city varchar(30) NOT NULL
);

INSERT INTO Container(type,name,active) VALUES
 ('rolcontainer', 'Wheeled Container',  true),
 ('emmer', 'Circular Bucket',  true),
 ('bag', 'Biodegradable wastebag',  true),
 ('box', 'Box',  true);

 INSERT INTO Size(size,
                 container_product_id,
                 discount_percentage,
                 unit_price_pickup,
                 unit_price_rent,
                 unit_price_placement)
  VALUES
 (140, 1, 0, 0, 0, 0),
 (10, 2, 0, 0, 0, 0),
 (20, 3, 5, 0, 0, 0),
 (130, 4, 5, 0, 0, 0);

 INSERT INTO Stream(type,
                 name,
                 active)
  VALUES
 ('glass', 'Glass', true),
 ('sinaasappelschillen', 'Orange Peels', true),
 ('koffiedrab', 'Coffee', true),
 ('blik', 'Cans', true);

 INSERT INTO LogisticalProvider(name)
  VALUES
 ('Retransport'),
 ('GreenCollect');

INSERT INTO PickupArea(PostalCodeFrom, PostalCodeTo)
  VALUES
 (1500, 2000),
 (1000, 1099),
 (1000, 1499);

 INSERT INTO ProviderPickupArea(PickupAreaId, LogisticalProviderId)
  VALUES
 (1, 1),
 (1, 2),
 (2, 1),
 (2, 2);

 INSERT INTO ProviderPickupArea_Stream(ProviderPickupAreaId, StreamId)
  VALUES
 (1, 1),
 (2, 2),
 (3, 1),
 (4, 2),
 (1, 3),
 (2, 4),
 (3, 2),
 (4, 1),
 (1, 4),
 (2, 3);

 INSERT INTO ProviderPickupArea_Container(ProviderPickupAreaId, ContainerId)
  VALUES
 (1, 1),
 (2, 2),
 (3, 1),
 (4, 2),
 (1, 2),
 (2, 1),
 (3, 3),
 (4, 4);

INSERT INTO ProviderPickupAreaDay(ProviderPickupAreaId, Day, WeekRecurrence)
  VALUES
 (1, 1, 1),
 (2, 2, 1),
 (3, 3, 2),
 (4, 4, 1),
 (1, 7, 1),
 (2, 1, 3),
 (3, 2, 1),
 (4, 3, 2),
 (1, 6, 1),
 (2, 7, 1),
 (3, 1, 1),
 (4, 2, 1);

INSERT INTO ProviderPickupAreaDayTimeSlot(ProviderPickupAreaDayId, PickupStart)
  VALUES
 (1, '08:00:00'),
 (2, '09:00:00'),
 (3, '10:00:00'),
 (4, '10:00:00'),
 (5, '10:00:00'),
 (6, '10:00:00'),
 (7, '10:00:00'),
 (8, '10:00:00'),
 (9, '10:00:00'),
 (10, '10:00:00'),
 (11, '10:00:00'),
 (12, '10:00:00'),
 (1, '10:00:00'),
 (2, '18:00:00'),
 (3, '10:00:00'),
 (4, '17:00:00'),
 (5, '10:00:00'),
 (6, '10:00:00'),
 (7, '15:00:00'),
 (8, '10:00:00'),
 (9, '11:30:00'),
 (10, '12:00:00'),
 (11, '10:00:00'),
 (12,'13:00:00');

 INSERT INTO StreamSize(StreamId, SizeId)
  VALUES
 (1, 1),
 (2, 2),
 (3, 3),
 (4, 4),
 (1, 4),
 (2, 3),
 (3, 2),
 (4, 1);

 INSERT INTO Customer(firstname, lastname, company, email, mobile, postalcode, street, streetnumber, city)
  VALUES
 ('Yuliya', 'Khadasevich', 'Coolblue', 'some.mail@gmail.com', '123456789', 3012, 'Weena', '664', 'Rotterdam');
