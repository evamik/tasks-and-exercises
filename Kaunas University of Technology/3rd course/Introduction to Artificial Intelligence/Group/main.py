from sklearn.cluster import KMeans
from sklearn.metrics import silhouette_score, silhouette_samples
from sklearn.preprocessing import StandardScaler
from sklearn_som.som import SOM
import pandas as pd
from pathlib import Path
import matplotlib.pyplot as plt
import matplotlib.cm as cm
import numpy as np
import os


def get_kmeans(scaled_features, start, end):
    inertias = []
    scores = []
    for n in range(start, end):
        kmeans = KMeans(
            n_clusters=n,
            init="random"
        )
        kmeans.fit_predict(scaled_features)
        inertias.append(kmeans.inertia_)
        scores.append(silhouette_score(
            scaled_features, kmeans.labels_).round(3))

    return inertias, scores


def plot_inertias(inertias, start, end):
    plt.plot(range(start, end), inertias, marker="o")
    plt.xticks(range(start, end, 2))
    plt.xlabel("Klasterių skaičius")
    plt.ylabel("Inercija")
    plt.title("Inercijos priklausomybė nuo klasterių skaičiaus")
    plt.grid()
    plt.savefig("kmeans/inertias.png")
    plt.clf()


def plot_scores(scores, start, end):
    plt.plot(range(start, end), scores, marker="o")
    plt.xticks(range(start, end, 2))
    plt.xlabel("Klasterių skaičius")
    plt.ylabel("Silueto koeficientas")
    plt.title("Silueto koeficiento priklausomybė nuo klasterių skaičiaus")
    plt.grid()
    plt.savefig("kmeans/scores.png")
    plt.clf()


def plot_silhouettes(scaled_features, cluster_labels, feature_1, feature_2):
    for n in cluster_labels:
        kmeans = KMeans(
            n_clusters=n
        )
        labels = kmeans.fit_predict(scaled_features)
        silhouette_avg = silhouette_score(scaled_features, labels)
        sample_silhouette_values = silhouette_samples(scaled_features, labels)

        fig, (ax1, ax2) = plt.subplots(1, 2)
        fig.set_size_inches(18, 7)

        ax1.set_xlim([-0.1, 1])
        ax1.set_ylim([0, len(scaled_features) + (n + 1) * 10])

        y_lower = 10
        for i in range(n):
            ith_cluster_silhouette_values = \
                sample_silhouette_values[labels == i]

            ith_cluster_silhouette_values.sort()

            size_cluster_i = ith_cluster_silhouette_values.shape[0]
            y_upper = y_lower + size_cluster_i

            color = cm.nipy_spectral(float(i) / n)
            ax1.fill_betweenx(np.arange(y_lower, y_upper),
                              0, ith_cluster_silhouette_values,
                              facecolor=color, edgecolor=color, alpha=0.7)

            ax1.text(-0.05, y_lower + 0.5 * size_cluster_i, str(i))

            y_lower = y_upper + 10

        ax1.set_title("Siluetų grafikai įvairiems klasteriams.")
        ax1.set_xlabel("Siluetų koeficientų reikšmės")
        ax1.set_ylabel("Klasteris")

        ax1.axvline(x=silhouette_avg, color="red", linestyle="--")

        ax1.set_yticks([])
        ax1.set_xticks([-0.1, 0, 0.2, 0.4, 0.6, 0.8, 1])

        colors = cm.nipy_spectral(labels.astype(float) / n)
        ax2.scatter(scaled_features[:, feature_1], scaled_features[:, feature_2], marker='.', s=30, lw=0, alpha=0.7,
                    c=colors, edgecolor='k')

        centers = kmeans.cluster_centers_
        ax2.scatter(centers[:, feature_1], centers[:, feature_2], marker='o',
                    c="white", alpha=1, s=200, edgecolor='k')

        for i, c in enumerate(centers):
            ax2.scatter(c[feature_1], c[feature_2], marker='$%d$' % i, alpha=1,
                        s=50, edgecolor='k')

        ax2.set_title("Klasterių vizualizacija")

        plt.suptitle(("Siluetų analizė su %d klasteriais" % n),
                     fontsize=14, fontweight='bold')

        plt.savefig("kmeans/silhouettes_%dvs%d-%d.png" %
                    (feature_1, feature_2, n))
    plt.clf()


def plot_som(scaled_features, m, feature_1, feature_2):
    som = SOM(m=m, n=1, dim=15)
    som.fit(scaled_features)
    predictions = som.predict(scaled_features)

    x = scaled_features[:, feature_1]
    y = scaled_features[:, feature_2]
    colors = cm.nipy_spectral(predictions.astype(
        float) / len(som.cluster_centers_))

    plt.scatter(x, y, c=colors, s=4)
    plt.title('Klasterių vizualizacija')
    plt.savefig("som/som_%dvs%d-%d.png" % (feature_1, feature_2, m))
    plt.clf()


def main():
    df = pd.read_csv(
        Path(__file__).resolve().parent.__str__()+"\\data.csv", ",")

    scaler = StandardScaler()
    scaled_features = scaler.fit_transform(
        df.drop(["track", "artist", "uri"], axis=1))

    start = 2
    end = 30

    inertias, scores = get_kmeans(scaled_features, start, end)

    if not os.path.exists('kmeans'):
        os.mkdir("kmeans")
    plot_inertias(inertias, start, end)
    plot_scores(scores, start, end)

    cluster_labels = [2, 4, 7]

    plot_silhouettes(scaled_features, cluster_labels, 1, 2)
    plot_silhouettes(scaled_features, cluster_labels, 1, 4)

    if not os.path.exists('som'):
        os.mkdir("som")
    plot_som(scaled_features, 2, 1, 2)
    plot_som(scaled_features, 4, 1, 2)
    plot_som(scaled_features, 7, 1, 2)
    plot_som(scaled_features, 2, 1, 4)
    plot_som(scaled_features, 4, 1, 4)
    plot_som(scaled_features, 7, 1, 4)


if __name__ == "__main__":
    main()
