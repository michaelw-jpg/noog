namespace Noog_api.Mappers
{
    public static class GenericMapper
    {
        public static void ApplyPatch<TEntity, TDto>(TEntity entity, TDto dto)
            where TEntity : class
            where TDto : class
        {
            var dtoProps = typeof(TDto).GetProperties();
            var entityProps = typeof(TEntity).GetProperties();
            foreach (var dtoProp in dtoProps)
            {
                var value = dtoProp.GetValue(dto);
                if (value == null)
                {
                    continue; //skip if a property is null
                }
                var entityProp = entityProps.FirstOrDefault(p => p.Name == dtoProp.Name);
                if (entityProp == null)
                    throw new Exception($"Property missmatch between {typeof(TEntity).Name} and {typeof(TDto).Name}"); //this shouldnt happen only if dto is missconfigured

                var targetType = Nullable.GetUnderlyingType(entityProp.PropertyType) ?? entityProp.PropertyType;
                entityProp.SetValue(entity, Convert.ChangeType(value, targetType));
            }
        }

        public static void ApplyCreate<TEntity, TDto>(TEntity entity, TDto dto)
            where TEntity : class
            where TDto : class
        {
            var dtoProps = typeof(TDto).GetProperties();
            var entityProps = typeof(TEntity).GetProperties();

            foreach (var dtoProp in dtoProps)
            {
                var value = dtoProp.GetValue(dto);
                var entityProp = entityProps.FirstOrDefault(p => p.Name == dtoProp.Name);
                if (entityProp == null)
                    throw new Exception($"Property missmatch between {typeof(TEntity).Name} and {typeof(TDto).Name}"); //this shouldnt happen only if dto is missconfigured
                if (value == null)
                    continue; // skip if a property is null, optional fields

                entityProp = entityProps.First(p => p.Name == dtoProp.Name);
                entityProp.SetValue(entity, value);
            }
        }
        public static TDto ToDto<TEntity, TDto>(TEntity entity)
            where TEntity : class
            where TDto : class, new()
        {
            if(entity == null) 
                return null;
            var dto = new TDto();
            var entityProps = typeof(TEntity).GetProperties();
            var dtoProps = typeof(TDto).GetProperties();

            foreach (var dtoProp in dtoProps)
            {
                var entityProp = entityProps.FirstOrDefault(p => p.Name == dtoProp.Name);
                if (entityProp == null)
                    throw new Exception($"Property missmatch between {typeof(TEntity).Name} and {typeof(TDto).Name}");

                var value = entityProp.GetValue(entity);
                if (value == null) continue; // skip null values

                var targetType = Nullable.GetUnderlyingType(dtoProp.PropertyType) ?? dtoProp.PropertyType;
                dtoProp.SetValue(dto, Convert.ChangeType(value, targetType));
            }
            return dto;
        }

        public static List<TDto> ToDtoList<TEntity, TDto>(IEnumerable<TEntity> entities)
            where TEntity : class
            where TDto : class, new()
        {
            if (entities == null) return new List<TDto>();

            var list = new List<TDto>();
            foreach (var entity in entities)
            {
                list.Add(ToDto<TEntity, TDto>(entity));
            }
            return list;
        }
    }


}

