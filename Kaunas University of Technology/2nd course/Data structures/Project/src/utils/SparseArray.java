package utils;

public class SparseArray<E> implements ISparseArray<E> {

    private int[] keys;
    private Object[] values;
    private int size = 0;
    private final int INCREMENT_SIZE = 3;
    private int arraySize;


    public SparseArray(){
        this(0);
    }

    public SparseArray(int size){
        keys = new int[size];
        values = new Object[size];
        arraySize = size;
    }

    public SparseArray(E[] array, E emptyValue){
        this();
        for(int i = 0; i < array.length; i++) {
            if (array[i] != emptyValue && array[i] != null) {
                put(i, array[i]);
            }
        }
    }

    public E get(int key){
        return get(key, null);
    }

    @SuppressWarnings("unchecked")
    public E get(int key, E defaultValue){
        int i = indexOfKey(key);

        if(i < 0)
            return defaultValue;
        else
            return (E)values[i];
    }

    public E remove(int key) {
        int i = binarySearch(key);

        if(i >= 0)
            return removeAt(i);

        return null;
    }

    @SuppressWarnings("unchecked")
    public E removeAt(int index){
        if(index >= size || index < 0)
            throw new ArrayIndexOutOfBoundsException(index);

        if(arraySize > size + INCREMENT_SIZE)
            resizeArray(arraySize - INCREMENT_SIZE);

        E val = (E)values[index];

        for(int i = index; i < size-1; i++){
            keys[i] = keys[i+1];
            values[i] = values[i+1];
        }

        size--;
        values[size] = null;

        return val;
    }

    @SuppressWarnings("unchecked")
    public E[] removeRange(int start, int endExclusive){
        if(start >= size)
            throw new ArrayIndexOutOfBoundsException(start);
        if(start >= endExclusive)
            throw new ArrayIndexOutOfBoundsException();

        E[] values = (E[]) new Object[endExclusive-start];
        int index = 0;

        for(int i = start; i < endExclusive; i++){
            values[index] = removeAt(i);
            index++;
            i--;
            endExclusive--;
        }

        return values;
    }

    public void put(int key, E value) {
        int i = binarySearch(key);

        if(i >= 0){
            values[i] = value;
        }
        else {
            i = ~i;
            size++;
            if(size > arraySize)
                resizeArray(arraySize + INCREMENT_SIZE);

            for(int j = size; j > i+1; j--){
                keys[j] = keys[j-1];
                values[j] = values[j-1];
            }
            keys[i] = key;
            values[i] = value;
        }
    }

    private void resizeArray(int newSize){
        int oldSize = keys.length;
        int[] oldKeys = keys.clone();
        if(oldSize > newSize)
            oldSize = newSize;
        keys = new int[newSize];
        System.arraycopy(oldKeys, 0, keys, 0, oldSize);
        Object[] oldValues = values.clone();
        values = new Object[newSize];
        System.arraycopy(oldValues, 0, values, 0, oldSize);
        arraySize = newSize;
    }

    private int binarySearch(int key){
        int first = 0;
        int last = size - 1;
        while (first <= last) {
            final int mid = (first + last) >>> 1;
            final int midVal = keys[mid];
            if (midVal < key) {
                first = mid + 1;
            } else if (midVal > key) {
                last = mid - 1;
            } else {
                return mid;  // value found
            }
        }
        return ~first;  // value not present
    }

    public int size(){
        return size;
    }

    public int keyAt(int index){
        if(index >= size)
            throw new ArrayIndexOutOfBoundsException(index);
        return keys[index];
    }

    @SuppressWarnings("unchecked")
    public E valueAt(int index){
        if(index >= size)
            throw new ArrayIndexOutOfBoundsException(index);
        return (E)values[index];
    }

    public void setValueAt(int index, E value){
        if(index >= size)
            throw new ArrayIndexOutOfBoundsException(index);

        values[index] = value;
    }

    public int indexOfKey(int key){
        return binarySearch(key);
    }

    public int indexOfValue(E value){
        for(int i = 0; i < size; i++)
            if(values[i] == value)
                return i;
        return -1;
    }

    public void clear(){
        keys = new int[INCREMENT_SIZE];
        values = new Object[INCREMENT_SIZE];
        arraySize = INCREMENT_SIZE;
        size = 0;
    }

    public long getMemorySize(){
        long bytes = size*4 + 12; // keys array
        bytes += 12; // primitive-type variables
        bytes += size*16 + 12; // values
        return bytes;
    }

    public String toArrayString(E defaultValue){
        StringBuilder sb = new StringBuilder();
        sb.append("{");

        for(int i = 0; i < keys[size-1]; i++){
            sb.append(String.format(" (%s),", get(i, defaultValue).toString()));
        }
        sb.deleteCharAt(sb.length()-1);
        sb.append(" }");

        return sb.toString();
    }

    @Override
    public String toString(){
        StringBuilder sb = new StringBuilder();

        for(int i = 0; i < size; i++){
            sb.append(String.format("%4d : \"%s\"\n", keys[i], values[i].toString()));
        }

        return sb.toString();
    }
}
