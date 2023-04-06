using Unity.Netcode;

namespace BossArena.game
{
    /// <summary>
    /// An example of a custom type serialized for use in RPC calls. This represents the state of a player as far as NGO is concerned,
    /// with relevant fields copied in or modified directly.
    /// </summary>
    public class PlayerData : INetworkSerializable
    {
        public string name;
        public ulong id;
        public Archetypes archetype;
        public PlayerData() { } // A default constructor is explicitly required for serialization.
        public PlayerData(string name, ulong id, Archetypes archetype = default) { this.name = name; this.id = id; this.archetype = archetype; }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref name);
            serializer.SerializeValue(ref id);
            serializer.SerializeValue(ref archetype);
        }
    }
}
