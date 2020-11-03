import java.nio.ByteBuffer

class DataObject(var pointer: Int, var string: String, var float: Float) : Comparable<DataObject> {
    override fun compareTo(other: DataObject): Int {
        if(string.compareTo(other.string) == 0)
            return float.compareTo(other.float)
        return string.compareTo(other.string)
    }

    override fun toString(): String {
        return "$string  $float"
    }

    fun to12Bytes(): ByteArray {
        val bytes = ByteArray(12)
        string.toByteArray(Charsets.UTF_16).copyOfRange(2,10).copyInto(bytes, 0)
        ByteBuffer.allocate(4).putFloat(float).array().copyInto(bytes, 8)
        return bytes
    }

    fun to16Bytes(): ByteArray {
        val bytes = ByteArray(16)
        string.toByteArray(Charsets.UTF_16).copyOfRange(2,10).copyInto(bytes, 0)
        ByteBuffer.allocate(4).putFloat(float).array().copyInto(bytes, 8)
        ByteBuffer.allocate(4).putInt(pointer).array().copyInto(bytes, 12)
        return bytes
    }
}