<?php // Do not put any HTML above this line

if (isset($_POST['cancel'])) {
    header("Location: index.php");
    return;
}

$salt = 'XyZzy12*_';
$stored_hash = '1a52e17fa899cf40fb04cfc42e6352f1';  // Pw is meow123

session_start();
// Check to see if we have some POST data, if we do process it
if (isset($_POST['email']) && isset($_POST['pass'])) {
    if (strlen($_POST['email']) < 1 || strlen($_POST['pass']) < 1) {
        $_SESSION['failure'] = "User name and password are required";
        header("Location: login.php");
        return;
    } else if (preg_match('/@/', $_POST['email']) == 0) {
        $_SESSION['failure'] = "Email must have an at-sign (@)";
        header("Location: login.php");
        return;
    } else {
        $check = hash('md5', $salt . $_POST['pass']);
        if ($check == $stored_hash) {
            // Redirect the browser to game.php
            $_SESSION['name'] = $_POST['email'];
            header("Location: view.php");
            return;
        } else {
            $_SESSION['failure'] = "Incorrect password";
            header("Location: login.php");
            return;
        }
    }
}

// Fall through into the View
?>
<!DOCTYPE html>
<html>

<head>
    <?php require_once "bootstrap.php"; ?>
    <title>Evaldas Mikalauskas's Login Page</title>
</head>

<body>
    <div class="container">
        <h1>Please Log In</h1>
        <?php
        if (isset($_SESSION['failure'])) {
            echo ('<p style="color: red;">' . htmlentities($_SESSION['failure']) . "</p>\n");
            unset($_SESSION['failure']);
        }
        ?>
        <form method="POST">
            <label for="nam">User Name</label>
            <input type="text" name="email" id="nam"><br />
            <label for="id_1723">Password</label>
            <input type="text" name="pass" id="id_1723"><br />
            <input type="submit" value="Log In">
            <input type="submit" name="cancel" value="Cancel">
        </form>
        <p>
            For a password hint, view source and find a password hint
            in the HTML comments.
            <!-- Hint: The password is the three letter programming language (all lower case) followed by 123. -->
        </p>
    </div>
</body>