# https://onlinejudge.org/index.php?option=com_onlinejudge&Itemid=8&category=29&page=show_problem&problem=1130
from pathlib import Path


class Cell:
    def __init__(self, is_mine):
        self.__is_mine = is_mine
        self.__neighbours = []

    def add_neighbour(self, cell, is_callback=False):
        self.__neighbours.append(cell)
        if not is_callback:
            cell.add_neighbour(self, True)

    def is_mine(self):
        return self.__is_mine

    def count_mines(self):
        count = 0
        for cell in self.__neighbours:
            if cell.is_mine():
                count += 1
        return count


class Grid:
    def __init__(self, rows, cols):
        self.__rows = rows
        self.__cols = cols
        self.__cells = [[None for i in range(cols)] for j in range(rows)]

    def get_cells(self):
        return self.__cells

    def set_cell(self, i, j, cell):
        self.__cells[i][j] = cell
        if i > 0:
            cell.add_neighbour(self.__cells[i-1][j])
            if j > 0:
                cell.add_neighbour(self.__cells[i-1][j-1])
            if j < self.__cols-1:
                cell.add_neighbour(self.__cells[i-1][j+1])
        if j > 0:
            cell.add_neighbour(self.__cells[i][j-1])

    def print(self):
        for i in range(self.__rows):
            row = ""
            for j in range(self.__cols):
                cell = self.__cells[i][j]
                if(cell is None):
                    row += "?"
                else:
                    if cell.is_mine():
                        row += "*"
                    else:
                        row += str(cell.count_mines())
            print(row)


def read_grids_from_file(filename):
    grids = []
    try:
        with open(Path(__file__).resolve().parent.__str__()+"\\"+filename) as file:
            while True:
                line = file.readline()
                numbers = line.split(" ")

                rows = int(numbers[0])
                cols = int(numbers[1])

                if rows == 0 and cols == 0:
                    return grids

                grid = Grid(rows, cols)

                for i in range(rows):
                    row = file.readline()
                    for j in range(cols):
                        cell = None
                        if row[j] == "*":
                            cell = Cell(True)
                        elif row[j] == ".":
                            cell = Cell(False)
                        grid.set_cell(i, j, cell)

                grids.append(grid)
    except Exception:
        print("Failed to read grids from file")


if __name__ == "__main__":
    grids = read_grids_from_file("input.txt")
    for i in range(len(grids)):
        print("Field #%d:" % (i+1))
        grids[i].print()
        print()
