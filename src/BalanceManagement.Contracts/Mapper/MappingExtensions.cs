namespace OwnerPropertyManagement.Contracts.Mapper
{
    public static class MappingExtensions
    {
        public static TDest MapTo<TDest>(this object src)
        {
            return ObjectMapper.Mapper.Map<TDest>(src);
        }
    }
}
