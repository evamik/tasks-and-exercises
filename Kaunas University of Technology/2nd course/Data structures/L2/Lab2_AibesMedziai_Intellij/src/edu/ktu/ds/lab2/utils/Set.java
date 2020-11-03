package edu.ktu.ds.lab2.utils;

/**
 * Interfeisu aprašomas Aibės ADT.
 *
 * @param <E> Aibės elemento duomenų tipas
 */
public interface Set<E> extends Iterable<E> {

    //Patikrinama ar aibė tuščia.
    boolean isEmpty();

    // Grąžinamas aibėje esančių elementų kiekis.
    int size();

    // Išvaloma aibė.
    void clear();

    // Aibė papildoma nauju elementu.
    void add(E element);

    // Aibė papildoma nauja elementų kolekcija.
    boolean addAll(BstSet<? extends E> c);

    //
    boolean removeAll(BstSet<? extends E> c);

    //
    SortedSet<? extends E> headSet(E toElement, boolean inclusive);

    //
    E lower(E e);

    //
    E higher(E e);

    //
    E last();

    //
    E pollLast();

    //
    E floor(E e);

    // Pašalinamas elementas iš aibės.
    void remove(E element);

    // Patikrinama ar elementas egzistuoja aibėje.
    boolean contains(E element);

    // Grąžinamas aibės elementų masyvas.
    Object[] toArray();

    // Gražinamas vizualiai išdėstytas aibės elementų turinys
    String toVisualizedString(String dataCodeDelimiter);
}
