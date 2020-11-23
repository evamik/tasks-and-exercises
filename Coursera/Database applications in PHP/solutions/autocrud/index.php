<?php
session_start();
if (isset($_SESSION['name'])) {
    require_once "pdo.php";

    $stmt = $pdo->query("SELECT * FROM autos ORDER BY make");
    $rows = $stmt->fetchAll(PDO::FETCH_ASSOC);
}
?>
<!DOCTYPE html>
<html>

<head>
    <title>Evaldas Mikalauskas - Autos Database</title>
    <?php require_once "bootstrap.php"; ?>
</head>

<body>
    <div class="container">
        <h1>Welcome to Automobiles Database</h1>
        <?php
        if (isset($_SESSION['failure'])) {
            echo ('<p style="color: red;">' . htmlentities($_SESSION['failure']) . "</p>\n");
            unset($_SESSION['failure']);
        }
        if (isset($_SESSION['success'])) {
            echo ('<p style="color: green;">' . htmlentities($_SESSION['success']) . "</p>\n");
            unset($_SESSION['success']);
        }
        if (!isset($_SESSION['name'])) {
            echo '  <p>
                        <a href="login.php">Please log in</a>
                    </p>
                    <p>Attempt to 
                        <a href="add.php">add data</a> without logging in</p>';
        } else {
            if (count($rows) == 0) {
                echo '<p>No rows found</p>';
            } else {
                echo '
                <table border="1">
                    <thead>
                        <tr>
                            <th>Make</th>
                            <th>Model</th>
                            <th>Year</th>
                            <th>Mileage</th>
                            <th>Action</th>
                        </tr>
                    </thead>';
                foreach ($rows as $row) {
                    echo '
                    <tr>
                        <td>' . htmlentities($row['make']) . '</td>
                        <td>' . htmlentities($row['model']) . '</td>
                        <td>' . htmlentities($row['year']) . '</td>
                        <td>' . htmlentities($row['mileage']) . '</td>
                        <td>
                            <a href="edit.php?autos_id=' . $row['autos_id'] . '">Edit</a>
                            /
                            <a href="delete.php?autos_id=' . $row['autos_id'] . '">Delete</a>
                        </td>
                    </tr>';
                }
                echo '
                </table>';
            }
            echo '
            <p><a href="add.php">Add New Entry</a></p>
            <p><a href="logout.php">Logout</a></p>';
        }
        ?>
    </div>
</body>