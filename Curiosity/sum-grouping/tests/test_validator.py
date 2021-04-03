import unittest
from main import read_from_file


class TestReadFromFile(unittest.TestCase):
    def test_file_not_found(self):
        filepath = "tests/duomenys-non-existant.csv"

        result = read_from_file(filepath, verbose=False)

        self.assertTrue(result.empty)

    def test_file_content_empty(self):
        filepath = "tests/duomenys-empty.csv"

        result = read_from_file(filepath, verbose=False)

        self.assertTrue(result.empty)

    def test_file_content_invalid(self):
        filepath = "tests/duomenys-read-invalid.csv"

        result = read_from_file(filepath, verbose=False)

        self.assertTrue(result.empty)
