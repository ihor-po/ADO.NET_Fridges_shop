USE "master"
GO

CREATE DATABASE "fridge_shop";
GO

USE "fridge_shop"
GO

CREATE TABLE "fridges"
(
	"id" INT IDENTITY (1,1) PRIMARY KEY,
	"mark" NVARCHAR(30) NOT NULL,
	"model" NVARCHAR(30) NOT NULL,
	"artikle" NVARCHAR(15) NOT NULL UNIQUE,
	"price" MONEY NOT NULL
);

INSERT INTO "fridges" VALUES
('SAMSUNG', 'SM-100500','151515AS156', 195.95),
('SAMSUNG', 'BR-1000','AE4447788', 99.95),
('ДНЕПР', 'DP-1','98798798', 19.95),
('NONAME', 'NA','147147147', 1199.95);

CREATE TABLE "sallers"
(
	"id" INT IDENTITY (1,1) PRIMARY KEY,
	"fio" NVARCHAR(50) NOT NULL,
	"inn" NVARCHAR(13) NOT NULL UNIQUE
);

INSERT INTO "sallers" VALUES
('Иванов Иван Иванович', 1234567890123),
('Иванова Иванна Ивановна', 1112223334445),
('Петров Петр Ильич', 6665554443210);

CREATE TABLE "buyers"
(
	"id" INT IDENTITY (1,1) PRIMARY KEY,
	"fio" NVARCHAR(50) NOT NULL,
	"address" NVARCHAR(50) NOT NULL,
	"phone" NVARCHAR(12)
);

INSERT INTO "buyers" VALUES
('Пупиткин Верислав Вениаминович', '67 Колугина, кв 65, пгт Пердыщев', '380128565741'),
('Лисабонская Маргарита Петровна', '123 Мышаковский проспект, кв 25, Каличград', '380225468745'),
('Брюс Вейн', 'Готем сити', '911'),
('Рамусин Каламбаджарун Марнивскаяч', '8 Маганбаджаран, Контри-сити', '380255544211');

CREATE TABLE "storage"
(
	"id" INT IDENTITY (1,1) PRIMARY KEY,
	"id_fridge" INT NOT NULL REFERENCES fridges(id) ON DELETE CASCADE,
	"quantity" INT NOT NULL DEFAULT 0
);

INSERT INTO "storage" VALUES
(1, 100),
(2, 2),
(3, 22),
(4, 5);

CREATE TABLE "cash_voucher"
(
	"id" INT IDENTITY (1,1) PRIMARY KEY,
	"seller_id" INT NOT NULL REFERENCES sallers(id) ON DELETE CASCADE,
	"buyer_id" INT NOT NULL REFERENCES buyers(id) ON DELETE CASCADE,
	"date" DATE NOT NULL
);

CREATE TABLE "cash_voucer_item"
(
	"id" INT IDENTITY (1,1) PRIMARY KEY,
	"cash_voucher_id" INT NOT NULL REFERENCES cash_voucher(id) ON DELETE CASCADE,
	"id_fridge" INT NOT NULL REFERENCES fridges(id) ON DELETE CASCADE,
	"quantity" INT NOT NULL DEFAULT 1
);