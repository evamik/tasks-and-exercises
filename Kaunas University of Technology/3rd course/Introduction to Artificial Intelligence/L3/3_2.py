import numpy as np
from pathlib import Path
import pandas as pd
from numpy import loadtxt
from keras.models import Sequential
from keras.layers import Dense
from sklearn.model_selection import KFold


def read_people_from_file(filename) -> pd.DataFrame:
    try:
        df = pd.DataFrame()
        df = pd.read_csv(
            Path(__file__).resolve().parent.__str__()+"\\"+filename, ";")
        return df
    except Exception:
        print("Failed to read data from file")


if __name__ == "__main__":
    df = loadtxt(Path(__file__).resolve().parent.__str__() +
                 "\\"+"cardio_train.csv", delimiter=";")
    print(len(df))
    X = df[:, 0:11]
    Y = df[:, 11]

    foldK = KFold(n_splits=10, shuffle=True)

    fd = 1

    accuracies = np.array([])

    for train, test in foldK.split(X, Y):

        model = Sequential()
        model.add(Dense(12, input_dim=11, activation='relu'))
        model.add(Dense(8, activation='relu'))
        model.add(Dense(1, activation='sigmoid'))
        model.compile(loss='binary_crossentropy',
                      optimizer='adam', metrics=['accuracy'])
        model.fit(X, Y, epochs=100, batch_size=10, verbose=0)
        _, accuracy = model.evaluate(X, Y)
        print("Accuracy: %.2f" % (accuracy*100))
        accuracies = np.append(accuracies, [(accuracy * 100)])
        fd += 1

    print("Average accuracy from %d runs:  %.2f" %
          (fd-1, np.average(accuracies)))
