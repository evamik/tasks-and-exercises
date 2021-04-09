import unittest
from classes.validator import Validator
from classes.dataframe import DataFrame


class TestValidator(unittest.TestCase):

    def test_dataframe_is_valid(self):
        df = DataFrame(
            {
                "Darbuotojas": ["Zuokas", "Dalia Grybauskaitė"],
                "Tipas": ["Alga", "Atostoginiai"],
                "Alga": [20, 50]
            })

        result = Validator.dataframe_is_valid(df)

        self.assertTrue(result)

    def test_dataframe_missing_column(self):
        df = DataFrame(
            {
                "Darbuotojas": ["Zuokas", "Dalia Grybauskaitė"],
                "Alga": [20, 50]
            })

        result = Validator.dataframe_is_valid(df)

        self.assertFalse(result)

    def test_dataframe_alga_NAN(self):
        df = DataFrame(
            {
                "Darbuotojas": ["Zuokas", "Dalia Grybauskaitė"],
                "Tipas": ["Alga", "Atostoginiai"],
                "Alga": ["NAN", 50]
            })

        result = Validator.dataframe_is_valid(df)

        self.assertFalse(result)
