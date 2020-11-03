package utils;

public interface ISparseArray<E> {

    /**
     * Grąžina elementą su raktu arba null, jei nėra
     * @param key raktas
     * @return Grąžina elementą su raktu arba null, jei nėra
     */
    E get(int key);

    /**
     * Grąžina elementą su raktu arba defaultValue, jei nėra
     * @param key raktas
     * @param defaultValue reikmė, jei nėra
     * @return Grąžina elementą su raktu arba defaultValue, jei nėra
     */
    E get(int key, E defaultValue);

    /**
     * Pašalina elementą su raktu
     * @param key raktas
     * @return Grąžina pašalintą elementą
     */
    E remove(int key);

    /**
     * Pašalina elementą indekso vietoje
     * @param index indeksas
     * @return Grąžina pašalintą elementą
     */
    E removeAt(int index);

    /**
     * Pašalina elementus intervale [start; endExclusive)
     * @param start intervalo pradžios indeksas (imtinai)
     * @param endExclusive intervalo pabaigos indeksas (neimtinai)
     * @return Grąžina pašalintų elementų mąsyvą
     */
    E[] removeRange(int start, int endExclusive);

    /**
     * Įdeda rakto ir elemento porą
     * @param key raktas
     * @param value elementas
     */
    void put(int key, E value);

    /**
     * Grąžina raktų-elementų kiekį
     * @return Grąžina raktų-elementų kiekį
     */
    int size();

    /**
     * Grąžina rakto reikšmę indekse
     * @param index indeksas
     * @return Grąžina rakto reikšmę indekse
     */
    int keyAt(int index);

    /**
     * Grąžina elemento reikšmę indekse
     * @param index indeksas
     * @return Grąžina elemento reikšmę indekse
     */
    E valueAt(int index);

    /**
     * Nustato elemento reikšmę indekse
     * @param index indeksas
     * @param value elementas
     */
    void setValueAt(int index, E value);

    /**
     * Grąžina rakto indeksą
     * @param key raktas
     * @return Grąžina rakto indeksą
     */
    int indexOfKey(int key);

    /**
     * Grąžina elemento indeksą
     * @param value elementas
     * @return Grąžina elemento indeksą
     */
    int indexOfValue(E value);

    /**
     * Išvalo raktus ir elementus
     */
    void clear();

    /**
     * Grąžina klasės objekto atminties užimtimo dydį (bytes)
     * @return Grąžina klasės objekto atminties užimtimo dydį (bytes)
     */
    long getMemorySize();

    /**
     * Grąžina klasės objektą suformatuotą spausdinimui kaip SparceArray
     * @return Grąžina klasės objektą suformatuotą spausdinimui kaip SparceArray
     */
    String toString();

    /**
     * Grąžina klasės objektą suformuotą spausdinimui kaip Array
     * @param defaultValue elemento reikšmė kur jo nėra
     * @return Grąžina klasės objektą suformuotą spausdinimui kaip Array
     */
    String toArrayString(E defaultValue);
}
