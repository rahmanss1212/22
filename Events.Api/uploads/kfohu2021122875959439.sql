-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 20, 2021 at 10:55 AM
-- Server version: 10.4.22-MariaDB
-- PHP Version: 8.0.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `college`
--

-- --------------------------------------------------------

--
-- Table structure for table `assessmenttypes`
--

CREATE TABLE `assessmenttypes` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `mark` decimal(10,0) NOT NULL,
  `courseid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `assessmenttypes`
--

INSERT INTO `assessmenttypes` (`id`, `name`, `mark`, `courseid`) VALUES
(2, 'fdadfhs', '567', 1),
(3, 'assess2', '23', 1),
(4, '123', '231', 1),
(5, 'assignemnt', '10', 1),
(6, 'final project', '10', 1);

-- --------------------------------------------------------

--
-- Table structure for table `attendance`
--

CREATE TABLE `attendance` (
  `slotid` int(11) NOT NULL,
  `studentid` int(11) NOT NULL,
  `status` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `attendance`
--

INSERT INTO `attendance` (`slotid`, `studentid`, `status`) VALUES
(14, 1, 'Abesnt'),
(14, 2, 'Abesnt'),
(15, 1, 'Attend'),
(15, 2, 'Abesnt'),
(16, 1, 'Attend'),
(16, 2, 'Abesnt'),
(17, 1, 'Attend'),
(17, 2, 'Attend'),
(17, 3, 'Abesnt');

-- --------------------------------------------------------

--
-- Table structure for table `attend_slot`
--

CREATE TABLE `attend_slot` (
  `id` int(11) NOT NULL,
  `topic` varchar(50) NOT NULL,
  `fromtime` time NOT NULL,
  `totime` time NOT NULL,
  `ldate` date NOT NULL,
  `lab` int(11) NOT NULL,
  `courseid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `attend_slot`
--

INSERT INTO `attend_slot` (`id`, `topic`, `fromtime`, `totime`, `ldate`, `lab`, `courseid`) VALUES
(14, 'fdhsdfhs', '19:23:00', '20:26:00', '2021-12-15', 0, 0),
(15, 'rthtrfhse', '18:28:00', '17:28:00', '2021-12-15', 567, 0),
(16, 'dfgadfg', '19:43:00', '17:44:00', '2021-12-15', 4654, 1),
(17, '46246', '15:30:00', '14:30:00', '2021-12-24', 45, 1);

-- --------------------------------------------------------

--
-- Table structure for table `course`
--

CREATE TABLE `course` (
  `id` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `weight` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `course`
--

INSERT INTO `course` (`id`, `name`, `weight`) VALUES
(1, '567', 467);

-- --------------------------------------------------------

--
-- Table structure for table `faculty`
--

CREATE TABLE `faculty` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `name` varchar(100) NOT NULL,
  `mobile` bigint(20) NOT NULL,
  `certificate` varchar(50) NOT NULL,
  `qualification` varchar(100) NOT NULL,
  `university` varchar(100) NOT NULL,
  `experience` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `faculty`
--

INSERT INTO `faculty` (`id`, `username`, `password`, `name`, `mobile`, `certificate`, `qualification`, `university`, `experience`) VALUES
(1, 'sss', '123', 'ahmed', 568756784, 'BCH', 'fgcbnnsdng', 'fgvnsfgn', 56);

-- --------------------------------------------------------

--
-- Table structure for table `facultycourseallocation`
--

CREATE TABLE `facultycourseallocation` (
  `courseid` int(11) NOT NULL,
  `facultyid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `facultycourseallocation`
--

INSERT INTO `facultycourseallocation` (`courseid`, `facultyid`) VALUES
(1, 1),
(1, 1);

-- --------------------------------------------------------

--
-- Table structure for table `marks`
--

CREATE TABLE `marks` (
  `id` int(11) NOT NULL,
  `studentId` int(11) NOT NULL,
  `assessid` int(11) NOT NULL,
  `result` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `marks`
--

INSERT INTO `marks` (`id`, `studentId`, `assessid`, `result`) VALUES
(14, 1, 2, '435'),
(15, 1, 3, '12'),
(16, 1, 4, '12'),
(17, 1, 5, '9'),
(18, 1, 6, '5'),
(19, 2, 2, '43'),
(20, 2, 3, '56'),
(21, 2, 4, '456'),
(22, 2, 5, '65'),
(23, 2, 6, '46'),
(24, 3, 2, '546'),
(25, 3, 3, '54'),
(26, 3, 4, '34'),
(27, 3, 5, '54'),
(28, 3, 6, '34');

-- --------------------------------------------------------

--
-- Table structure for table `students`
--

CREATE TABLE `students` (
  `studentid` int(11) NOT NULL,
  `civilid` int(11) NOT NULL,
  `fullname` varchar(100) NOT NULL,
  `mobile` bigint(20) NOT NULL,
  `qualification` varchar(50) NOT NULL,
  `institute` varchar(50) NOT NULL,
  `grade` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `students`
--

INSERT INTO `students` (`studentid`, `civilid`, `fullname`, `mobile`, `qualification`, `institute`, `grade`) VALUES
(1, 67456, 'ahmed', 5674568, 'coputner', 'sfghsfg', '56'),
(2, 654867, 'gsfghfgh', 3456345, 'gsdfhsfgh', 'gfhsfgh', '354676'),
(3, 567, 'fgdhsfh', 564768, 'fghsfgh', 'fghsfgh', '5678');

-- --------------------------------------------------------

--
-- Table structure for table `student_course_enrollment`
--

CREATE TABLE `student_course_enrollment` (
  `courseid` int(11) NOT NULL,
  `studentid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `student_course_enrollment`
--

INSERT INTO `student_course_enrollment` (`courseid`, `studentid`) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 1),
(1, 1),
(1, 2),
(1, 2),
(1, 3),
(1, 3);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `assessmenttypes`
--
ALTER TABLE `assessmenttypes`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `attend_slot`
--
ALTER TABLE `attend_slot`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `course`
--
ALTER TABLE `course`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `faculty`
--
ALTER TABLE `faculty`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `marks`
--
ALTER TABLE `marks`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `students`
--
ALTER TABLE `students`
  ADD PRIMARY KEY (`studentid`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `assessmenttypes`
--
ALTER TABLE `assessmenttypes`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `attend_slot`
--
ALTER TABLE `attend_slot`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- AUTO_INCREMENT for table `course`
--
ALTER TABLE `course`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `faculty`
--
ALTER TABLE `faculty`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `marks`
--
ALTER TABLE `marks`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT for table `students`
--
ALTER TABLE `students`
  MODIFY `studentid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
