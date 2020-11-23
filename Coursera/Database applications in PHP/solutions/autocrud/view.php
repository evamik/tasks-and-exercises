<?php
session_start();
if (!isset($_SESSION['name'])) {
    die("Not logged in");
}
require_once "pdo.php";


if (isset($_POST['logout'])) {
    header('Location:index.php');
}

$stmt = $pdo->query("SELECT * FROM autos ORDER BY make");
$rows = $stmt->fetchAll(PDO::FETCH_ASSOC);
?>
<!DOCTYPE html>
<html>

<head>
    <title>Evaldas Mikalauskas's Automobile Tracker</title>

    <?php require_once "bootstrap.php"; ?>

</head>

<body>
    <div class="container">
        <h1>Tracking Autos for <?= htmlentities($_SESSION['name']) ?></h1>
        <?php
        if (isset($_SESSION['success'])) {
            echo ('<p style="color: green;">' . htmlentities($_SESSION['success']) . "</p>\n");
            unset($_SESSION['success']);
        }
        ?>
        <h2>Automobiles</h2>
        <?php
        foreach ($rows as $row) {
            echo "<li>";
            echo (htmlentities($row['year']));
            echo " ";
            echo (htmlentities($row['make']));
            echo " / ";
            echo (htmlentities($row['mileage']));
            echo "</li>";
        }
        ?>
        <a href="add.php">Add New</a>
        |
        <a href="logout.php">Logout</a>
    </div>

</html>