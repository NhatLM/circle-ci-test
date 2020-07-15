CREATE DATABASE  IF NOT EXISTS `accounts` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `accounts`;
-- MySQL dump 10.13  Distrib 8.0.20, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: accounts
-- ------------------------------------------------------
-- Server version	8.0.20

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cust_user`
--

DROP TABLE IF EXISTS `cust_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cust_user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `cust_id` int DEFAULT NULL,
  `user_type` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL DEFAULT 'USER',
  `active` int NOT NULL DEFAULT '0',
  `name` varchar(50) DEFAULT NULL,
  `title` varchar(255) DEFAULT NULL,
  `email` varchar(60) DEFAULT NULL,
  `phone_country_code` int DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `direct_phone_country_code` int DEFAULT NULL,
  `direct_phone` varchar(20) DEFAULT NULL,
  `locale` varchar(2) NOT NULL DEFAULT 'da' COMMENT 'ISO 639-1',
  `admin_locale` varchar(2) DEFAULT NULL,
  `hash` varchar(13) DEFAULT NULL,
  `password` varchar(128) DEFAULT NULL,
  `user_level` int DEFAULT '1000',
  `allow_create_users` int DEFAULT NULL,
  `allow_bidding` int DEFAULT NULL,
  `allow_selling` int DEFAULT NULL,
  `allow_guarenteed_bid` int DEFAULT NULL,
  `allow_buyer_block` int DEFAULT NULL,
  `assessment_user` int DEFAULT NULL,
  `stay_on_hash` varchar(255) DEFAULT NULL,
  `list_view_mode` enum('list','gallery') NOT NULL DEFAULT 'list',
  `last_daily_auction` datetime DEFAULT NULL,
  `invoice_free` date DEFAULT NULL,
  `terms_approval` datetime DEFAULT NULL,
  `currency_code` varchar(3) DEFAULT NULL,
  `news_read` date DEFAULT NULL,
  `confirm_favorite_removal` int NOT NULL DEFAULT '0',
  `block_usage` int DEFAULT NULL,
  `remember_token` varchar(100) DEFAULT NULL,
  `invoice_contact_name` varchar(100) DEFAULT NULL,
  `erp_id_number` varchar(30) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `group_email` varchar(255) DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  `cust_task_substitute` int DEFAULT NULL,
  `email_verified_at` timestamp NULL DEFAULT NULL,
  `emailplatform_id` bigint DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `hash` (`hash`),
  KEY `cust` (`cust_id`),
  KEY `cust_id` (`cust_id`,`stay_on_hash`),
  KEY `fk_cust_user_customer_user_types` (`user_type`),
  KEY `fk_cust_user_phone_country_codes` (`phone_country_code`),
  KEY `fk_cust_user_direct_phone_country_codes` (`direct_phone_country_code`)
) ENGINE=InnoDB AUTO_INCREMENT=55765 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cust_user`
--

LOCK TABLES `cust_user` WRITE;
/*!40000 ALTER TABLE `cust_user` DISABLE KEYS */;
INSERT INTO `cust_user` VALUES (1,NULL,'USER',0,'test','test title','test@mail.com',NULL,NULL,NULL,NULL,'da',NULL,NULL,'fed3b61b26081849378080b34e693d2e',0,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'list',NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,'2020-07-09 03:45:35',NULL,NULL,NULL,NULL,NULL),(2,NULL,'USER',0,'system',NULL,'system@mail.com',NULL,NULL,NULL,NULL,'da',NULL,NULL,'c5c5f475a05592d2e09d010c9789f899',5,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'list',NULL,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,'2020-07-09 03:46:36',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `cust_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `id` int unsigned NOT NULL AUTO_INCREMENT,
  `level` int DEFAULT NULL,
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci NOT NULL,
  `created_at` timestamp NULL DEFAULT NULL,
  `updated_at` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` VALUES (1,0,'Developer',NULL,NULL),(2,5,'System',NULL,NULL);
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-07-09 17:56:41
