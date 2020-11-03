-- phpMyAdmin SQL Dump
-- version 3.4.11.1deb2+deb7u8
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Mar 03, 2020 at 10:37 AM
-- Server version: 1.0.35
-- PHP Version: 5.6.37-1~dotdeb+zts+7.1

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `evamik3`
--

-- --------------------------------------------------------

--
-- Table structure for table `BILL`
--

CREATE TABLE IF NOT EXISTS `BILL` (
  `number` int(11) NOT NULL,
  `date` date NOT NULL,
  `sum` double NOT NULL,
  `fk_CONTRACT` int(11) NOT NULL,
  PRIMARY KEY (`number`),
  KEY `fkc_CONTRACT` (`fk_CONTRACT`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `BILL`
--

INSERT INTO `BILL` (`number`, `date`, `sum`, `fk_CONTRACT`) VALUES
(0, '2020-01-15', 234.45, 0),
(1, '2020-01-15', 194.91, 0),
(2, '2019-03-03', 211.87, 3),
(3, '2019-10-19', 182.75, 3),
(4, '2019-09-03', 218.39, 0),
(5, '2019-10-02', 207.94, 1),
(6, '2019-08-12', 214.21, 3),
(7, '2019-03-19', 160.91, 1),
(8, '2019-11-10', 206.58, 2);

-- --------------------------------------------------------

--
-- Table structure for table `CITY`
--

CREATE TABLE IF NOT EXISTS `CITY` (
  `name` varchar(255) NOT NULL,
  `city_type` int(11) NOT NULL,
  `id_CITY` int(11) NOT NULL,
  PRIMARY KEY (`id_CITY`),
  UNIQUE KEY `name` (`name`),
  KEY `city_type` (`city_type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `CITY`
--

INSERT INTO `CITY` (`name`, `city_type`, `id_CITY`) VALUES
('Cropalati', 1, 0),
('Harlech', 1, 1),
('Cockburn', 1, 2),
('Paularo', 2, 3),
('Andernach', 3, 4);

-- --------------------------------------------------------

--
-- Table structure for table `CITY_TYPE`
--

CREATE TABLE IF NOT EXISTS `CITY_TYPE` (
  `id_CITY_TYPE` int(11) NOT NULL,
  `name` char(7) NOT NULL,
  PRIMARY KEY (`id_CITY_TYPE`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `CITY_TYPE`
--

INSERT INTO `CITY_TYPE` (`id_CITY_TYPE`, `name`) VALUES
(1, 'village'),
(2, 'town'),
(3, 'city');

-- --------------------------------------------------------

--
-- Table structure for table `CLIENT`
--

CREATE TABLE IF NOT EXISTS `CLIENT` (
  `personal_code` bigint(20) NOT NULL,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `birth_date` date NOT NULL,
  `phone_number` int(11) NOT NULL,
  `email` varchar(255) NOT NULL,
  `client_since` date NOT NULL,
  PRIMARY KEY (`personal_code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `CLIENT`
--

INSERT INTO `CLIENT` (`personal_code`, `name`, `surname`, `birth_date`, `phone_number`, `email`, `client_since`) VALUES
(11444673261, 'Raya', 'Reeves', '1968-01-23', 860485751, 'orci.Ut@dignissimtemporarcu.co.uk', '2015-02-18'),
(11971519353, 'Kellie', 'Acevedo', '1995-06-03', 860053799, 'cursus.Integer@acfacilisis.ca', '2012-12-18'),
(12238940439, 'Robin', 'Bonner', '1976-01-07', 869557282, 'nonummy.ut@gravida.ca', '2011-11-29'),
(12482484923, 'Ruth', 'Emerson', '1967-09-22', 869537427, 'Curabitur@cursus.com', '2013-09-11'),
(14779289274, 'Kyle', 'Lester', '1983-04-29', 860185597, 'Nunc.sed@eu.net', '2015-04-27'),
(15166669482, 'Laura', 'Goff', '1985-10-18', 865922583, 'montes.nascetur@Etiamgravidamolestie.com', '2016-07-11'),
(18194084662, 'Lila', 'Porter', '1958-05-17', 863671065, 'sagittis.lobortis.mauris@tristiquenequevenenatis.co.uk', '2012-08-05'),
(19642419419, 'Joan', 'Lamb', '1961-04-22', 866942077, 'mauris@idnunc.co.uk', '2018-09-11'),
(19677679125, 'Martina', 'Estes', '1978-03-13', 861568120, 'in.lobortis@sem.org', '2019-06-10'),
(19938740778, 'Alana', 'Wilcox', '1999-05-08', 869104339, 'mauris.ut@sodalesatvelit.ca', '2018-10-25'),
(21556851576, 'Stacy', 'Craft', '1968-12-22', 869671892, 'eu@sagittisaugue.ca', '2019-08-16'),
(22389245823, 'Lenore', 'Molina', '1975-09-23', 860540902, 'cursus@ametfaucibusut.org', '2018-12-15'),
(23202860111, 'Charde', 'Patton', '1969-11-02', 863025274, 'nascetur.ridiculus.mus@faucibus.co.uk', '2013-04-06'),
(24957569174, 'Stacey', 'Daugherty', '1975-08-17', 861287046, 'tristique.neque@antedictum.com', '2009-09-29'),
(26339423777, 'Wade', 'Buckley', '1980-12-23', 864951314, 'Ut.nec@erat.net', '2014-06-08'),
(29159436887, 'Giselle', 'Snider', '1998-08-17', 861020169, 'vitae.erat.Vivamus@adipiscingfringillaporttitor.co.uk', '2017-06-03'),
(30329685963, 'Nash', 'Bender', '1957-10-22', 868438997, 'vehicula.et.rutrum@quisarcuvel.org', '2012-11-20'),
(32405613526, 'Zahir', 'Horne', '1980-02-09', 861582466, 'nulla.ante.iaculis@cursusdiamat.co.uk', '2015-03-14'),
(32535333479, 'Tyrone', 'Butler', '1962-10-20', 861896551, 'ut.aliquam@scelerisquedui.co.uk', '2012-06-20'),
(32544619382, 'Quyn', 'Patton', '1977-10-05', 861778483, 'Sed.eu.eros@intempuseu.edu', '2010-05-01'),
(33707582791, 'Melodie', 'Rollins', '1971-06-24', 867877309, 'nascetur.ridiculus@lectusa.org', '2012-12-11'),
(34377288299, 'Shelby', 'Mcguire', '1981-08-17', 860855700, 'Maecenas@Curabitur.edu', '2012-12-21'),
(36911992657, 'Wade', 'Bonner', '1972-11-14', 868710704, 'ligula.Donec@urna.net', '2015-11-04'),
(37385933492, 'Maxine', 'Pollard', '1987-12-26', 860062201, 'pretium.aliquet.metus@velquamdignissim.org', '2014-02-05'),
(37584511472, 'Charissa', 'Lucas', '2001-02-26', 865627997, 'sodales.nisi@PhasellusornareFusce.co.uk', '2009-03-23'),
(38498713336, 'Lavinia', 'Stephenson', '1999-08-07', 865388306, 'sagittis@nonlobortis.org', '2012-09-23'),
(39433661458, 'Tobias', 'Delaney', '1958-07-10', 867136404, 'mi.Duis@Donectincidunt.net', '2012-02-26'),
(42333380349, 'Ayanna', 'Buckner', '1962-10-11', 867250098, 'diam.luctus@semsempererat.co.uk', '2012-02-18'),
(42914155687, 'Giacomo', 'Webster', '1998-12-31', 865091514, 'pellentesque.massa.lobortis@sitametmetus.org', '2012-02-22'),
(43436473148, 'Lenore', 'Compton', '1969-12-23', 868918615, 'posuere.enim@felisadipiscingfringilla.ca', '2019-11-28'),
(43709878903, 'Ginger', 'Cunningham', '1994-11-07', 865110201, 'nunc@mus.org', '2014-11-27'),
(44533551962, 'Herrod', 'Robbins', '1960-02-08', 869189206, 'amet.ultricies.sem@Aeneansedpede.co.uk', '2014-10-20'),
(45524988889, 'Maggie', 'Hines', '1996-09-01', 865964772, 'erat.volutpat@laoreetliberoet.com', '2018-09-13'),
(47662880911, 'Elijah', 'Shepard', '1958-08-04', 864997564, 'ultricies.adipiscing.enim@a.ca', '2013-04-12'),
(47937753485, 'Zoe', 'Bullock', '1960-05-20', 860928505, 'at@imperdieteratnonummy.org', '2012-09-09'),
(48543984385, 'Jamal', 'Lewis', '1987-09-28', 866521622, 'vestibulum.lorem@arcuVestibulum.edu', '2019-10-22'),
(48915052463, 'Garrison', 'Whitfield', '1982-06-23', 866016835, 'Aliquam.tincidunt@Sed.ca', '2019-12-25'),
(50759564654, 'Jaime', 'Lindsey', '1984-07-04', 861137726, 'magnis.dis@dolortempus.edu', '2009-07-22'),
(51314170794, 'Karly', 'Oneal', '1963-05-21', 866631712, 'aliquet.nec.imperdiet@dictum.edu', '2012-07-27'),
(51498648857, 'Delilah', 'Wilkins', '1979-07-26', 860503790, 'purus.Nullam@ullamcorper.org', '2019-12-23'),
(51609643944, 'Rahim', 'Shannon', '1989-06-25', 865801985, 'ac.mattis@ligulaconsectetuerrhoncus.ca', '2010-02-25'),
(51902560819, 'Stephen', 'Moreno', '1958-04-01', 863880841, 'Vestibulum.ante.ipsum@cubiliaCuraePhasellus.com', '2015-06-28'),
(55413215967, 'Elvis', 'Nixon', '1960-10-31', 866467694, 'euismod@facilisisloremtristique.edu', '2019-05-03'),
(57279639195, 'Sigourney', 'Herring', '1971-05-17', 863831614, 'lorem@ac.co.uk', '2017-02-12'),
(57384241689, 'Jonah', 'Holmes', '1982-06-25', 861099708, 'elit.dictum@tortornibhsit.com', '2013-11-20'),
(57527544192, 'Lani', 'Cline', '1981-09-02', 863108469, 'mattis@ametdiameu.net', '2012-07-13'),
(57568368231, 'Maile', 'Baker', '1979-02-11', 867608555, 'eget.mollis@Donecfeugiat.net', '2015-02-07'),
(57771191421, 'Gillian', 'Ellis', '1973-07-08', 862048839, 'elementum@maurisidsapien.ca', '2017-03-02'),
(58986915231, 'Wyoming', 'Anderson', '1992-09-21', 860241242, 'eu.odio@risusNulla.co.uk', '2009-06-23'),
(59599587505, 'Aaron', 'Ewing', '1988-08-16', 868978007, 'in@sitametrisus.net', '2019-12-02'),
(59647677407, 'Tara', 'Gomez', '1962-01-17', 866817919, 'non.enim@risus.org', '2012-02-29'),
(59756989161, 'Wyatt', 'Malone', '1973-08-10', 866461458, 'mauris.rhoncus@tempus.com', '2015-07-25'),
(60553733923, 'Claudia', 'Powers', '1989-07-15', 863630218, 'Cum.sociis.natoque@acfeugiatnon.edu', '2011-12-23'),
(61732256455, 'Vernon', 'Pace', '1988-02-16', 864841465, 'diam.eu@maurissapiencursus.com', '2019-07-27'),
(63912181766, 'Zachery', 'Webb', '1957-10-10', 868800907, 'Cras.vulputate.velit@imperdietornareIn.org', '2017-05-25'),
(64261760806, 'Yuli', 'Gilliam', '1959-01-05', 866046844, 'aliquam.enim@tempus.org', '2018-06-19'),
(64475062822, 'Connor', 'Copeland', '1990-11-02', 861594686, 'est@ametdapibus.net', '2015-05-16'),
(65124376408, 'Marsden', 'Miller', '1989-08-31', 861655920, 'molestie.in@orci.com', '2012-04-03'),
(65684894163, 'Kristen', 'Daniels', '1994-05-06', 862105935, 'Lorem.ipsum.dolor@nibhlaciniaorci.ca', '2016-03-09'),
(67539973433, 'Reese', 'Mclaughlin', '1956-09-30', 862646323, 'Suspendisse.non.leo@non.co.uk', '2010-12-16'),
(67695144523, 'Travis', 'Berry', '1979-02-27', 865192090, 'augue@feugiat.ca', '2009-09-07'),
(67975312552, 'Hashim', 'May', '1967-12-21', 866696185, 'risus@Praesent.co.uk', '2013-02-11'),
(68123588124, 'Freya', 'Osborne', '1968-11-23', 867994411, 'aliquam.adipiscing.lacus@augue.com', '2013-11-20'),
(69438341278, 'Herrod', 'Boyd', '1992-03-30', 861735962, 'ultricies.ornare@ipsum.co.uk', '2009-06-10'),
(70835191752, 'Ali', 'Obrien', '1991-09-07', 862752085, 'Donec@Nulla.co.uk', '2019-10-18'),
(72116634247, 'Nissim', 'Terrell', '1955-07-11', 862366067, 'erat@massaQuisqueporttitor.co.uk', '2016-10-04'),
(73593837566, 'Herman', 'Everett', '1977-11-03', 863414484, 'egestas@penatibus.com', '2009-04-22'),
(74637326252, 'Gray', 'Avila', '1970-06-05', 865765882, 'id@volutpat.co.uk', '2019-06-17'),
(75175621167, 'Jada', 'Hendricks', '2000-04-16', 862733066, 'cursus@ipsum.ca', '2014-07-09'),
(77194857818, 'Allen', 'Case', '1984-02-20', 868930062, 'tellus.imperdiet.non@nonlobortisquis.com', '2016-10-18'),
(78641739531, 'Arden', 'Bernard', '1991-11-22', 864865425, 'vel.turpis.Aliquam@mus.com', '2016-09-16'),
(79106558403, 'Charlotte', 'Mccall', '1957-11-22', 867515760, 'arcu@suscipit.org', '2010-09-05'),
(79201384403, 'Guinevere', 'Obrien', '1974-02-08', 863587443, 'interdum.libero@nullaInteger.ca', '2015-08-10'),
(79254783626, 'Willow', 'Powell', '1992-03-25', 862742481, 'purus.gravida.sagittis@rhoncusNullamvelit.net', '2018-02-13'),
(79654931183, 'Kermit', 'Boyle', '1992-02-29', 863376240, 'lorem.lorem.luctus@aliquam.org', '2009-04-06'),
(84424099412, 'Britanney', 'Vang', '1978-11-16', 864293814, 'convallis@et.edu', '2018-09-18'),
(85434880121, 'Lionel', 'Espinoza', '1984-11-18', 864811155, 'Mauris.magna@famesac.com', '2020-01-19'),
(85535375573, 'Desirae', 'Bauer', '1997-01-24', 869777433, 'rutrum@egestas.ca', '2019-03-12'),
(85682521584, 'Carter', 'Lawrence', '1992-11-18', 865400219, 'Nullam@Integer.org', '2014-12-18'),
(86323421571, 'Aidan', 'Gill', '1960-07-23', 868426713, 'lobortis@sedfacilisis.edu', '2016-03-22'),
(86327070231, 'Randall', 'Sharp', '1990-04-12', 863158423, 'arcu.Aliquam.ultrices@Morbi.ca', '2016-04-23'),
(87289076698, 'Whitney', 'Browning', '1988-12-17', 868830808, 'orci.adipiscing@arcuVestibulumante.ca', '2017-08-18'),
(87508042707, 'Alec', 'Dillon', '1992-09-12', 865572585, 'Nulla.eget.metus@nondapibus.ca', '2012-02-03'),
(89172885474, 'Willa', 'Wolf', '1982-04-12', 861916600, 'dui.Fusce.aliquam@Sed.ca', '2015-02-08'),
(89351294658, 'Colleen', 'Fuentes', '1961-09-24', 867275416, 'odio.a@cursus.ca', '2017-03-19'),
(90871573265, 'Claudia', 'Watson', '1957-04-23', 867647235, 'sem.elit@Morbineque.ca', '2016-11-25'),
(92457578963, 'Gavin', 'Parker', '1957-11-10', 869315112, 'a@vel.net', '2018-03-18'),
(93722430711, 'Kylynn', 'Fowler', '1970-06-25', 861752023, 'Nam.ac@porttitorinterdumSed.org', '2009-03-25'),
(93851715726, 'Yeo', 'Wynn', '1963-03-19', 862706876, 'per.inceptos@a.net', '2016-04-12'),
(94618675444, 'Heidi', 'Townsend', '1993-03-10', 866930159, 'vitae@ultricies.ca', '2016-06-20'),
(95135654152, 'Zeus', 'Hodge', '1970-01-25', 867788346, 'dui.semper@sapiengravidanon.com', '2017-09-22'),
(95255259272, 'Rhonda', 'Hood', '1973-04-26', 867273958, 'penatibus@nullaatsem.org', '2015-08-05'),
(96534515134, 'Tamekah', 'Sheppard', '1991-04-29', 863607306, 'leo@risusDuisa.net', '2020-01-25'),
(97876058775, 'John', 'Colon', '1978-03-16', 863103074, 'lorem.lorem@magna.co.uk', '2010-04-24'),
(98228664623, 'Arden', 'Mccray', '1958-09-07', 867927451, 'Nulla.eu.neque@ac.net', '2019-10-10'),
(98475891632, 'Brian', 'Curtis', '1974-02-13', 865861581, 'aliquam.eros.turpis@augueeutempor.ca', '2013-04-22'),
(99177336219, 'Macey', 'Chavez', '1969-07-17', 862687703, 'neque@magnisdis.org', '2014-04-30'),
(99446368895, 'Baxter', 'Haney', '1970-05-12', 865470078, 'Cras@nonummyFuscefermentum.ca', '2019-08-23'),
(99492953316, 'Denton', 'Jordan', '1975-03-15', 861939771, 'eleifend.Cras@ultricesposuere.co.uk', '2015-10-21'),
(99529557757, 'Joan', 'Kerr', '1973-06-30', 863536309, 'cursus.luctus@magnased.co.uk', '2017-03-11');

-- --------------------------------------------------------

--
-- Table structure for table `CONTRACT`
--

CREATE TABLE IF NOT EXISTS `CONTRACT` (
  `id` int(11) NOT NULL,
  `order_date` date NOT NULL,
  `repair_start_date` date NOT NULL,
  `expected_end_date` date NOT NULL,
  `real_end_date` date NOT NULL,
  `sum` double NOT NULL,
  `additional_costs` double NOT NULL,
  `fk_WORKER` int(11) NOT NULL,
  `fk_CLIENT` bigint(20) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fkc_WORKER` (`fk_WORKER`),
  KEY `fkc_CLIENT` (`fk_CLIENT`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `CONTRACT`
--

INSERT INTO `CONTRACT` (`id`, `order_date`, `repair_start_date`, `expected_end_date`, `real_end_date`, `sum`, `additional_costs`, `fk_WORKER`, `fk_CLIENT`) VALUES
(0, '2018-07-08', '2018-07-08', '2018-07-13', '2018-07-14', 1581.15, 64.13, 94, 15166669482),
(1, '2015-03-04', '2015-03-04', '2015-01-04', '2015-01-04', 239.69, 122.19, 70, 12482484923),
(2, '2016-01-28', '2016-01-28', '2016-02-12', '2016-02-12', 2020.3, 9.73, 13, 11444673261),
(3, '2015-11-25', '2015-11-25', '2015-12-01', '2015-11-29', 2353.35, 90.87, 93, 12238940439);

-- --------------------------------------------------------

--
-- Table structure for table `GARAGE`
--

CREATE TABLE IF NOT EXISTS `GARAGE` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `address` varchar(255) NOT NULL,
  `built_date` date NOT NULL,
  `worker_capacity` int(11) NOT NULL,
  `phone` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `garage_type` int(11) NOT NULL,
  `fk_CITY` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `address` (`address`),
  KEY `garage_type` (`garage_type`),
  KEY `fkc_CITY` (`fk_CITY`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `GARAGE`
--

INSERT INTO `GARAGE` (`id`, `name`, `address`, `built_date`, `worker_capacity`, `phone`, `email`, `garage_type`, `fk_CITY`) VALUES
(0, 'Scelerisque Neque LLP', '5629 Pede. Road', '2019-07-10', 4, '868376523', 'lorem@dolordolor.org', 1, 4),
(1, 'Vel Faucibus Id Incorporated', 'P.O. Box 543, 5834 Nec, Avenue', '2020-06-13', 4, '865137833', 'semper@Suspendisseseddolor.com', 3, 4),
(2, 'Magnis Dis Parturient PC', '589-341 Ridiculus Street', '2020-09-21', 2, '865163902', 'mi.enim@neceuismod.ca', 2, 4),
(3, 'Rhoncus Proin Nisl LLP', '4720 Quisque St.', '2019-10-02', 5, '868031954', 'nunc.est@augueSed.ca', 1, 2),
(4, 'Vel Vulputate Foundation', 'P.O. Box 992, 2808 Dui. Ave', '2019-10-13', 4, '865841664', 'conubia.nostra@ut.ca', 1, 0),
(5, 'Tincidunt Foundation', 'Ap #249-4423 Luctus Rd.', '2019-08-22', 5, '861309828', 'sed.est.Nunc@atpede.net', 3, 4),
(6, 'Non Feugiat Industries', '1761 Sed Road', '2019-03-03', 4, '860664356', 'et@Nunc.org', 2, 1),
(7, 'Duis Consulting', 'Ap #667-4071 Integer Ave', '2020-09-20', 5, '864961860', 'nec.metus.facilisis@posuerecubilia.ca', 1, 0),
(8, 'Fusce Mi Lorem LLP', 'Ap #294-1284 Nunc, Rd.', '2019-07-05', 6, '868008771', 'tellus@sit.co.uk', 3, 2),
(9, 'Ipsum Consulting', 'P.O. Box 565, 223 Fringilla Av.', '2020-11-21', 5, '864312351', 'id@Etiamlaoreetlibero.ca', 3, 2),
(10, 'Dignissim Company', '3272 Odio Av.', '2020-08-23', 5, '866091181', 'non@quis.com', 2, 0),
(11, 'Et Inc.', '750 Et St.', '2020-12-06', 3, '867481623', 'nec.quam.Curabitur@sem.net', 1, 2),
(12, 'Vehicula Risus Company', '800-1970 Non, Av.', '2020-07-26', 5, '860808455', 'aliquet.libero.Integer@quisaccumsanconvallis.org', 3, 4),
(13, 'Ut Erat Sed Institute', '8874 Luctus Ave', '2019-08-13', 4, '868627716', 'id.ante@euismod.edu', 2, 1),
(14, 'Libero Dui Nec LLP', 'P.O. Box 955, 5518 Lorem Ave', '2020-06-10', 2, '865302156', 'placerat.velit@etmagnis.ca', 1, 1),
(15, 'Donec Est LLP', 'P.O. Box 986, 5509 Phasellus Street', '2019-06-30', 2, '865017342', 'mauris.Integer.sem@Donecdignissim.co.uk', 2, 1);

-- --------------------------------------------------------

--
-- Table structure for table `GARAGE_TYPE`
--

CREATE TABLE IF NOT EXISTS `GARAGE_TYPE` (
  `id_GARAGE_TYPE` int(11) NOT NULL,
  `name` char(17) NOT NULL,
  PRIMARY KEY (`id_GARAGE_TYPE`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `GARAGE_TYPE`
--

INSERT INTO `GARAGE_TYPE` (`id_GARAGE_TYPE`, `name`) VALUES
(1, 'repair shop'),
(2, 'installation shop'),
(3, 'all in one shop');

-- --------------------------------------------------------

--
-- Table structure for table `GENDER`
--

CREATE TABLE IF NOT EXISTS `GENDER` (
  `id_GENDER` int(11) NOT NULL,
  `name` char(6) NOT NULL,
  PRIMARY KEY (`id_GENDER`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `GENDER`
--

INSERT INTO `GENDER` (`id_GENDER`, `name`) VALUES
(1, 'male'),
(2, 'female'),
(3, 'other');

-- --------------------------------------------------------

--
-- Table structure for table `ORDERED_SERVICE`
--

CREATE TABLE IF NOT EXISTS `ORDERED_SERVICE` (
  `count` int(11) NOT NULL,
  `id_ORDERED_SERVICE` int(11) NOT NULL,
  `fk_SERVICE` int(11) NOT NULL,
  `fk_CONTRACT` int(11) NOT NULL,
  PRIMARY KEY (`id_ORDERED_SERVICE`),
  KEY `fkc_SERVICE` (`fk_SERVICE`),
  KEY `fkc_CONTRACT2` (`fk_CONTRACT`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `ORDERED_SERVICE`
--

INSERT INTO `ORDERED_SERVICE` (`count`, `id_ORDERED_SERVICE`, `fk_SERVICE`, `fk_CONTRACT`) VALUES
(3, 0, 5, 1),
(1, 1, 4, 2),
(2, 2, 5, 3),
(2, 3, 4, 2),
(3, 4, 2, 3),
(2, 5, 6, 0);

-- --------------------------------------------------------

--
-- Table structure for table `PARTS`
--

CREATE TABLE IF NOT EXISTS `PARTS` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `price` double NOT NULL,
  `count` int(11) NOT NULL,
  `fk_GARAGE` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`,`fk_GARAGE`),
  KEY `fkc_GARAGE` (`fk_GARAGE`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `PARTS`
--

INSERT INTO `PARTS` (`id`, `name`, `price`, `count`, `fk_GARAGE`) VALUES
(0, 'Injector5', 0, 13, 6),
(1, 'Injector3', 0, 19, 10),
(2, 'Injector3', 0, 17, 8),
(3, 'Spark plug3', 0, 9, 3),
(4, 'Engine5', 0, 15, 11),
(5, 'Spark plug4', 0, 10, 12),
(6, 'Engine3', 0, 9, 4),
(7, 'Spark plug2', 0, 11, 15),
(8, 'Spark plug2', 0, 12, 13),
(9, 'Injector3', 0, 18, 4),
(10, 'Spark plug2', 0, 8, 12),
(11, 'Spark plug3', 0, 16, 14),
(12, 'Spark plug3', 0, 16, 10),
(13, 'Spark plug4', 0, 16, 0),
(14, 'Injector4', 0, 18, 9),
(15, 'Injector5', 0, 8, 1),
(16, 'Injector1', 0, 8, 7),
(17, 'Spark plug5', 0, 19, 14),
(18, 'Engine3', 0, 14, 14),
(19, 'Spark plug1', 0, 20, 2),
(20, 'Engine4', 0, 9, 15),
(21, 'Spark plug5', 0, 20, 12),
(22, 'Spark plug2', 0, 11, 1),
(23, 'Spark plug2', 0, 9, 10),
(24, 'Injector2', 0, 11, 5),
(25, 'Spark plug5', 0, 16, 13),
(26, 'Spark plug3', 0, 7, 9),
(27, 'Injector3', 0, 18, 13),
(28, 'Injector2', 0, 16, 1),
(29, 'Engine5', 0, 10, 7),
(30, 'Engine5', 0, 9, 15),
(31, 'Engine4', 0, 14, 2),
(32, 'Engine3', 0, 12, 15),
(33, 'Engine3', 0, 6, 10),
(34, 'Engine2', 0, 12, 9),
(35, 'Injector5', 0, 17, 10),
(36, 'Injector4', 0, 6, 6),
(37, 'Injector1', 0, 5, 1),
(38, 'Spark plug1', 0, 13, 13),
(39, 'Engine1', 0, 10, 11),
(40, 'Injector1', 0, 20, 2),
(41, 'Spark plug4', 0, 4, 10),
(42, 'Spark plug1', 0, 7, 15),
(43, 'Engine1', 0, 8, 12),
(44, 'Injector4', 0, 6, 0),
(45, 'Engine4', 0, 9, 5),
(46, 'Engine3', 0, 14, 8),
(47, 'Engine3', 0, 18, 1),
(48, 'Engine1', 0, 8, 6),
(49, 'Engine3', 0, 12, 2),
(50, 'Engine1', 0, 14, 3),
(51, 'Engine2', 0, 10, 4),
(52, 'Engine1', 0, 19, 5),
(53, 'Spark plug2', 0, 12, 5),
(54, 'Injector3', 0, 7, 1),
(55, 'Injector2', 0, 11, 12),
(56, 'Spark plug4', 0, 14, 5);

-- --------------------------------------------------------

--
-- Table structure for table `PARTS_USED`
--

CREATE TABLE IF NOT EXISTS `PARTS_USED` (
  `id` int(11) NOT NULL,
  `count` int(11) NOT NULL,
  `fk_PARTS` int(11) NOT NULL,
  `fk_CONTRACT` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fkc_PARTS` (`fk_PARTS`),
  KEY `fkc_CONTRACT3` (`fk_CONTRACT`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `PARTS_USED`
--

INSERT INTO `PARTS_USED` (`id`, `count`, `fk_PARTS`, `fk_CONTRACT`) VALUES
(0, 4, 12, 0),
(1, 3, 29, 3),
(2, 3, 35, 0),
(3, 3, 51, 2),
(4, 4, 6, 0),
(5, 1, 9, 2),
(6, 1, 14, 2),
(7, 3, 31, 0),
(8, 4, 14, 3),
(9, 1, 28, 3),
(10, 1, 20, 3),
(11, 2, 7, 1),
(12, 4, 35, 2),
(13, 4, 4, 0),
(14, 2, 44, 3),
(15, 2, 16, 0),
(16, 4, 10, 2),
(17, 3, 46, 3),
(18, 3, 56, 3),
(19, 3, 34, 2);

-- --------------------------------------------------------

--
-- Table structure for table `PAYMENT`
--

CREATE TABLE IF NOT EXISTS `PAYMENT` (
  `date` date NOT NULL,
  `sum` double NOT NULL,
  `id_PAYMENT` int(11) NOT NULL,
  `fk_BILL` int(11) NOT NULL,
  `fk_CLIENT` bigint(20) NOT NULL,
  PRIMARY KEY (`id_PAYMENT`),
  KEY `fkc_BILL` (`fk_BILL`),
  KEY `fkc_CLIENT2` (`fk_CLIENT`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `PAYMENT`
--

INSERT INTO `PAYMENT` (`date`, `sum`, `id_PAYMENT`, `fk_BILL`, `fk_CLIENT`) VALUES
('2019-07-06', 217.41, 0, 3, 12482484923),
('2019-05-04', 231.18, 1, 6, 14779289274),
('2019-05-06', 261.77, 2, 7, 11444673261),
('2019-07-19', 183.28, 3, 0, 11444673261),
('2020-01-23', 193.74, 4, 7, 11971519353),
('2019-03-31', 235.11, 5, 8, 14779289274);

-- --------------------------------------------------------

--
-- Table structure for table `SERVICE`
--

CREATE TABLE IF NOT EXISTS `SERVICE` (
  `name` varchar(255) NOT NULL,
  `price` double NOT NULL,
  `duration_days` int(11) NOT NULL,
  `id_SERVICE` int(11) NOT NULL,
  PRIMARY KEY (`id_SERVICE`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `SERVICE`
--

INSERT INTO `SERVICE` (`name`, `price`, `duration_days`, `id_SERVICE`) VALUES
('Spark plug replace', 0, 4, 0),
('Spark plug install', 0, 6, 1),
('Engine replace', 0, 2, 2),
('Engine install', 0, 14, 3),
('Injector replace', 0, 2, 4),
('Inspection and repair', 0, 14, 5),
('Injector install', 0, 9, 6);

-- --------------------------------------------------------

--
-- Table structure for table `WORKER`
--

CREATE TABLE IF NOT EXISTS `WORKER` (
  `id` int(11) NOT NULL,
  `name` varchar(255) NOT NULL,
  `surname` varchar(255) NOT NULL,
  `phone_number` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `birth_date` date NOT NULL,
  `worker_status` int(11) NOT NULL,
  `gender` int(11) NOT NULL,
  `fk_GARAGE` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `worker_status` (`worker_status`),
  KEY `gender` (`gender`),
  KEY `fkc_GARAGE2` (`fk_GARAGE`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `WORKER`
--

INSERT INTO `WORKER` (`id`, `name`, `surname`, `phone_number`, `email`, `birth_date`, `worker_status`, `gender`, `fk_GARAGE`) VALUES
(0, 'Rae', 'Rush', '868893651', 'cursus.et@auctorvitae.net', '0000-00-00', 1, 1, 10),
(1, 'Briar', 'Irwin', '868140789', 'id@maurissagittisplacerat.edu', '0000-00-00', 2, 3, 5),
(2, 'Justin', 'Carter', '866740619', 'dictum.placerat.augue@Namac.com', '0000-00-00', 2, 2, 14),
(3, 'Asher', 'Meadows', '863701388', 'Proin.ultrices@velitinaliquet.edu', '0000-00-00', 1, 3, 14),
(4, 'Jared', 'Wheeler', '862018530', 'sit.amet@egestas.com', '0000-00-00', 2, 2, 3),
(5, 'Tallulah', 'Colon', '869785261', 'cursus.non.egestas@mattissemperdui.net', '0000-00-00', 3, 2, 4),
(6, 'Galvin', 'Morrison', '869786768', 'imperdiet.erat@tinciduntadipiscingMauris.net', '0000-00-00', 3, 3, 8),
(7, 'Liberty', 'Powell', '867480478', 'tempor.erat.neque@Lorem.ca', '0000-00-00', 3, 2, 10),
(8, 'Timon', 'Mccullough', '868515398', 'eu.odio.Phasellus@lacusvestibulum.net', '0000-00-00', 3, 1, 15),
(9, 'George', 'Mason', '862827139', 'faucibus.lectus@ac.org', '0000-00-00', 3, 3, 4),
(10, 'Lucian', 'Casey', '863727380', 'Sed.eu@tempordiamdictum.ca', '0000-00-00', 2, 2, 6),
(11, 'Cora', 'Bonner', '867688917', 'lobortis.ultrices@velitegestaslacinia.ca', '0000-00-00', 2, 1, 0),
(12, 'Hiram', 'Wilder', '863265182', 'id.sapien.Cras@lacus.com', '0000-00-00', 3, 2, 10),
(13, 'Mercedes', 'Pacheco', '865801928', 'at.egestas@non.net', '0000-00-00', 1, 1, 5),
(14, 'Mira', 'Blankenship', '862634291', 'ut@Suspendisse.ca', '0000-00-00', 1, 2, 3),
(15, 'Madeson', 'Wilkerson', '866187746', 'Nulla.eget@Vivamussitamet.ca', '0000-00-00', 2, 2, 8),
(16, 'Yuli', 'Huff', '862989401', 'in.faucibus@gravida.com', '0000-00-00', 1, 2, 5),
(17, 'Montana', 'Goodwin', '864028825', 'sociis@atvelit.co.uk', '0000-00-00', 2, 3, 4),
(18, 'Danielle', 'Dunn', '861897075', 'Donec@intempuseu.com', '0000-00-00', 3, 3, 7),
(19, 'Neville', 'Randall', '869981660', 'magna.Lorem.ipsum@laoreetipsum.org', '0000-00-00', 3, 1, 6),
(20, 'Nero', 'Bean', '864956005', 'mi.tempor@liberoatauctor.edu', '0000-00-00', 3, 1, 0),
(21, 'Chanda', 'Walters', '869021954', 'commodo.at.libero@Phasellusdapibusquam.co.uk', '0000-00-00', 3, 2, 0),
(22, 'Serena', 'Jennings', '865967604', 'rutrum.eu@nibhvulputate.edu', '0000-00-00', 1, 3, 15),
(23, 'Ingrid', 'Calderon', '867254061', 'quis.massa@dignissimMaecenasornare.ca', '0000-00-00', 1, 1, 11),
(24, 'Isaac', 'Mcgowan', '864709806', 'mollis@massaSuspendisseeleifend.ca', '0000-00-00', 2, 2, 12),
(25, 'Althea', 'Dotson', '868944622', 'In.mi.pede@elitNullafacilisi.co.uk', '0000-00-00', 1, 1, 13),
(26, 'Stone', 'Lane', '868935204', 'nisi.Cum@Phaselluslibero.edu', '0000-00-00', 1, 1, 7),
(27, 'Debra', 'Kemp', '866423764', 'amet.risus.Donec@a.org', '0000-00-00', 2, 1, 8),
(28, 'Jerry', 'Mccall', '867001209', 'a.tortor.Nunc@gravida.com', '0000-00-00', 3, 1, 0),
(29, 'Xander', 'Boyle', '861940474', 'non@rutrum.com', '0000-00-00', 1, 3, 0),
(30, 'Cyrus', 'Greene', '862028669', 'purus.ac@enimgravidasit.net', '0000-00-00', 3, 2, 8),
(31, 'Phoebe', 'Sanchez', '861510247', 'elit.sed.consequat@enimsit.com', '0000-00-00', 3, 3, 2),
(32, 'Cain', 'Daniels', '860872808', 'tellus.Nunc@ut.ca', '0000-00-00', 2, 2, 6),
(33, 'Jade', 'Dennis', '862831480', 'dui.Cum.sociis@justo.com', '0000-00-00', 1, 1, 1),
(34, 'Brooke', 'Graves', '862628074', 'Sed@iaculisaliquetdiam.net', '0000-00-00', 3, 1, 13),
(35, 'India', 'Hess', '862273820', 'lorem@Maurisut.ca', '0000-00-00', 1, 3, 6),
(36, 'Whoopi', 'Witt', '863750988', 'Sed@fermentum.org', '0000-00-00', 1, 3, 9),
(37, 'Venus', 'Obrien', '865258809', 'purus.Nullam.scelerisque@amalesuada.ca', '0000-00-00', 3, 3, 3),
(38, 'Reece', 'Sharpe', '861923772', 'pede.nonummy@imperdietornare.ca', '0000-00-00', 3, 3, 14),
(39, 'Fleur', 'Johnston', '860189070', 'consectetuer@fringilla.ca', '0000-00-00', 3, 3, 0),
(40, 'Rooney', 'Browning', '868534384', 'Quisque@amet.org', '0000-00-00', 3, 3, 10),
(41, 'Montana', 'Rose', '868324695', 'odio.auctor@pedePraesent.co.uk', '0000-00-00', 2, 3, 15),
(42, 'Wyoming', 'York', '868813668', 'ac.risus.Morbi@facilisislorem.co.uk', '0000-00-00', 2, 3, 9),
(43, 'Macaulay', 'Harmon', '867331361', 'libero@Curabitur.net', '0000-00-00', 2, 1, 3),
(44, 'Azalia', 'Zimmerman', '860072177', 'Phasellus.in@Morbi.ca', '0000-00-00', 1, 1, 8),
(45, 'Griffin', 'Maddox', '864943111', 'tempus.mauris@augue.net', '0000-00-00', 2, 1, 8),
(46, 'Preston', 'Burris', '867347497', 'Curabitur.sed.tortor@Curabitur.org', '0000-00-00', 1, 1, 8),
(47, 'Fredericka', 'Hernandez', '864274924', 'aliquet.lobortis@blandit.co.uk', '0000-00-00', 3, 3, 11),
(48, 'Deborah', 'Odom', '863233734', 'Cras.sed@odio.co.uk', '0000-00-00', 1, 2, 2),
(49, 'Carol', 'Duran', '869663371', 'Cras@ategestas.com', '0000-00-00', 2, 1, 8),
(50, 'Asher', 'Gilliam', '867727075', 'dui.nec@iaculis.edu', '0000-00-00', 2, 3, 12),
(51, 'Magee', 'Whitney', '868788894', 'lectus.pede.ultrices@maurisIntegersem.co.uk', '0000-00-00', 3, 3, 15),
(52, 'Thane', 'Drake', '860091375', 'eget.tincidunt@adipiscing.ca', '0000-00-00', 1, 2, 1),
(53, 'Iona', 'Adams', '860198885', 'scelerisque@metusVivamus.net', '0000-00-00', 3, 2, 1),
(54, 'Remedios', 'Marks', '862142679', 'erat@magnaUt.com', '0000-00-00', 1, 2, 7),
(55, 'Carissa', 'Stout', '863408296', 'dui.Cum.sociis@orciluctuset.net', '0000-00-00', 2, 3, 15),
(56, 'Yoshi', 'Conway', '863418109', 'risus.at.fringilla@est.co.uk', '0000-00-00', 2, 3, 0),
(57, 'Cailin', 'Brown', '860272249', 'risus@eu.net', '0000-00-00', 2, 1, 7),
(58, 'Ciara', 'Kirkland', '861097361', 'Morbi@ultricies.net', '0000-00-00', 1, 2, 4),
(59, 'Wing', 'Hill', '865311487', 'Nullam@mi.edu', '0000-00-00', 3, 1, 13),
(60, 'Kyla', 'Sparks', '860342107', 'Suspendisse.eleifend.Cras@Sedcongue.com', '0000-00-00', 1, 2, 15),
(61, 'Kirsten', 'Atkins', '863114321', 'nisl.Quisque.fringilla@duinec.net', '0000-00-00', 3, 2, 9),
(62, 'Harding', 'Head', '867594285', 'pede.ultrices.a@duisemperet.ca', '0000-00-00', 3, 3, 12),
(63, 'Aurelia', 'Valdez', '863328416', 'fringilla.ornare@sed.edu', '0000-00-00', 1, 1, 12),
(64, 'Daryl', 'Mays', '865745812', 'sed.dolor.Fusce@Duisgravida.co.uk', '0000-00-00', 2, 2, 1),
(65, 'Jemima', 'Workman', '860463358', 'tempor@lobortisnisinibh.net', '0000-00-00', 3, 2, 12),
(66, 'Kimberly', 'Rosario', '868121404', 'Nunc@id.org', '0000-00-00', 2, 3, 0),
(67, 'Brooke', 'Reynolds', '862220862', 'Proin@aliquam.ca', '0000-00-00', 2, 3, 11),
(68, 'Shelley', 'Grant', '869222274', 'amet.consectetuer@Crasvulputatevelit.org', '0000-00-00', 1, 1, 13),
(69, 'Chanda', 'Dodson', '865498045', 'sit.amet@Sedpharetrafelis.com', '0000-00-00', 2, 3, 11),
(70, 'Ronan', 'Hartman', '864085545', 'nulla.In.tincidunt@ut.com', '0000-00-00', 3, 2, 1),
(71, 'Rigel', 'Atkins', '860118796', 'fermentum.risus@velitAliquamnisl.net', '0000-00-00', 2, 2, 14),
(72, 'Coby', 'Lang', '866372541', 'lorem.ut@dictummagna.net', '0000-00-00', 3, 2, 6),
(73, 'Quin', 'Hunt', '862349222', 'In.nec.orci@pellentesque.net', '0000-00-00', 2, 2, 6),
(74, 'Harriet', 'Oneil', '863591433', 'mus.Donec.dignissim@cursus.org', '0000-00-00', 2, 3, 15),
(75, 'Travis', 'Valentine', '865846175', 'tellus.imperdiet.non@egestasa.edu', '0000-00-00', 3, 2, 14),
(76, 'Adrienne', 'Yang', '868924529', 'cursus.non.egestas@MorbimetusVivamus.net', '0000-00-00', 2, 2, 2),
(77, 'Paloma', 'Pittman', '869075668', 'nec.diam@congueaaliquet.ca', '0000-00-00', 1, 2, 12),
(78, 'Ariana', 'Whitley', '860763427', 'tincidunt.adipiscing.Mauris@maurissitamet.co.uk', '0000-00-00', 1, 2, 8),
(79, 'Hadley', 'Boyd', '867933740', 'Quisque.fringilla@acarcuNunc.ca', '0000-00-00', 2, 3, 6),
(80, 'Alfreda', 'Riley', '866997149', 'sed.dolor@cursusa.edu', '0000-00-00', 3, 3, 7),
(81, 'Amethyst', 'Jacobs', '867549225', 'et.ultrices.posuere@metusIn.edu', '0000-00-00', 1, 3, 4),
(82, 'Jolie', 'Cain', '865496074', 'nec@eget.net', '0000-00-00', 3, 2, 3),
(83, 'Cheyenne', 'Barron', '860204573', 'aliquet.vel@dictum.org', '0000-00-00', 2, 1, 13),
(84, 'Ulric', 'Rosa', '867279662', 'ullamcorper.magna@diamatpretium.com', '0000-00-00', 2, 1, 12),
(85, 'Aidan', 'Mckay', '869544815', 'lorem@iaculislacuspede.edu', '0000-00-00', 1, 3, 6),
(86, 'Devin', 'Benson', '864499566', 'tempus@dolorNulla.co.uk', '0000-00-00', 1, 3, 11),
(87, 'Brooke', 'Eaton', '865592208', 'hendrerit@velitSedmalesuada.org', '0000-00-00', 3, 3, 6),
(88, 'Rose', 'Rodgers', '868735394', 'Nulla.eu@nulla.net', '0000-00-00', 2, 2, 1),
(89, 'Cheyenne', 'Stone', '868465611', 'lacus.Mauris.non@purus.ca', '0000-00-00', 1, 3, 8),
(90, 'Bo', 'Brock', '866173693', 'dictum.sapien.Aenean@luctus.edu', '0000-00-00', 1, 3, 11),
(91, 'Wilma', 'Richards', '867408134', 'auctor@eratvitae.edu', '0000-00-00', 3, 2, 12),
(92, 'Bree', 'Garner', '864420738', 'enim@magnanecquam.co.uk', '0000-00-00', 1, 2, 3),
(93, 'Rigel', 'Norton', '861495242', 'Praesent.eu@feugiat.ca', '0000-00-00', 3, 3, 4),
(94, 'Gloria', 'Phelps', '862149035', 'lobortis@dapibus.org', '0000-00-00', 2, 2, 13),
(95, 'Sawyer', 'Skinner', '861875754', 'eu@lectus.edu', '0000-00-00', 2, 1, 12),
(96, 'Louis', 'Gonzales', '865236492', 'Vivamus.nisi.Mauris@morbi.net', '0000-00-00', 2, 3, 7),
(97, 'Mollie', 'Bennett', '868429930', 'est.mollis.non@dolordolortempus.com', '0000-00-00', 3, 2, 13),
(98, 'Celeste', 'Shelton', '865023343', 'sed@tellusfaucibus.edu', '0000-00-00', 2, 1, 14),
(99, 'Melyssa', 'Lara', '865898130', 'ipsum.Phasellus.vitae@convallisincursus.co.uk', '0000-00-00', 1, 1, 9);

-- --------------------------------------------------------

--
-- Table structure for table `WORKER_STATUS`
--

CREATE TABLE IF NOT EXISTS `WORKER_STATUS` (
  `id_WORKER_STATUS` int(11) NOT NULL,
  `name` char(15) NOT NULL,
  PRIMARY KEY (`id_WORKER_STATUS`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `WORKER_STATUS`
--

INSERT INTO `WORKER_STATUS` (`id_WORKER_STATUS`, `name`) VALUES
(1, 'working'),
(2, 'on holiday'),
(3, 'sick'),
(4, 'maternity leave');

--
-- Constraints for dumped tables
--

--
-- Constraints for table `BILL`
--
ALTER TABLE `BILL`
  ADD CONSTRAINT `fkc_CONTRACT` FOREIGN KEY (`fk_CONTRACT`) REFERENCES `CONTRACT` (`id`);

--
-- Constraints for table `CITY`
--
ALTER TABLE `CITY`
  ADD CONSTRAINT `CITY_ibfk_1` FOREIGN KEY (`city_type`) REFERENCES `CITY_TYPE` (`id_CITY_TYPE`);

--
-- Constraints for table `CONTRACT`
--
ALTER TABLE `CONTRACT`
  ADD CONSTRAINT `fkc_CLIENT` FOREIGN KEY (`fk_CLIENT`) REFERENCES `CLIENT` (`personal_code`),
  ADD CONSTRAINT `fkc_WORKER` FOREIGN KEY (`fk_WORKER`) REFERENCES `WORKER` (`id`);

--
-- Constraints for table `GARAGE`
--
ALTER TABLE `GARAGE`
  ADD CONSTRAINT `GARAGE_ibfk_1` FOREIGN KEY (`garage_type`) REFERENCES `GARAGE_TYPE` (`id_GARAGE_TYPE`),
  ADD CONSTRAINT `fkc_CITY` FOREIGN KEY (`fk_CITY`) REFERENCES `CITY` (`id_CITY`);

--
-- Constraints for table `ORDERED_SERVICE`
--
ALTER TABLE `ORDERED_SERVICE`
  ADD CONSTRAINT `fkc_CONTRACT2` FOREIGN KEY (`fk_CONTRACT`) REFERENCES `CONTRACT` (`id`),
  ADD CONSTRAINT `fkc_SERVICE` FOREIGN KEY (`fk_SERVICE`) REFERENCES `SERVICE` (`id_SERVICE`);

--
-- Constraints for table `PARTS`
--
ALTER TABLE `PARTS`
  ADD CONSTRAINT `fkc_GARAGE` FOREIGN KEY (`fk_GARAGE`) REFERENCES `GARAGE` (`id`);

--
-- Constraints for table `PARTS_USED`
--
ALTER TABLE `PARTS_USED`
  ADD CONSTRAINT `fkc_CONTRACT3` FOREIGN KEY (`fk_CONTRACT`) REFERENCES `CONTRACT` (`id`),
  ADD CONSTRAINT `fkc_PARTS` FOREIGN KEY (`fk_PARTS`) REFERENCES `PARTS` (`id`);

--
-- Constraints for table `PAYMENT`
--
ALTER TABLE `PAYMENT`
  ADD CONSTRAINT `fkc_BILL` FOREIGN KEY (`fk_BILL`) REFERENCES `BILL` (`number`),
  ADD CONSTRAINT `fkc_CLIENT2` FOREIGN KEY (`fk_CLIENT`) REFERENCES `CLIENT` (`personal_code`);

--
-- Constraints for table `WORKER`
--
ALTER TABLE `WORKER`
  ADD CONSTRAINT `WORKER_ibfk_1` FOREIGN KEY (`worker_status`) REFERENCES `WORKER_STATUS` (`id_WORKER_STATUS`),
  ADD CONSTRAINT `WORKER_ibfk_2` FOREIGN KEY (`gender`) REFERENCES `GENDER` (`id_GENDER`),
  ADD CONSTRAINT `fkc_GARAGE2` FOREIGN KEY (`fk_GARAGE`) REFERENCES `GARAGE` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
