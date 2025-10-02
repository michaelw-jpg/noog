namespace Noog_api.Mappers
{
    public static class GenericMapper
    {

        /// <summary>
        /// Applies non null values from Dto to an existing entity, updating only the fields that are provided in the Dto.
        /// Properties in the Dto that are are null are ignored, leaving the corresponding entity properties unchanged.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity to be updated.</typeparam>
        /// <typeparam name="TDto">The type of the DTO containing the values to patch.</typeparam>
        /// <param name="entity">The entity instance to be updated</param>
        /// <param name="dto">The DTO containing values to apply to the entity</param>
        /// <exception cref="Exception">Thrown if a property in the DTO does not exist on the entity.</exception>
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

        /// <summary>
        /// Applies values from a DTO to a new or existing entity.
        /// Null properties in the DTO are ignored, allowing for optional fields during creation.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity to be created</typeparam>
        /// <typeparam name="TDto">The type of DTO containing the values. </typeparam>
        /// <param name="entity">The entity instance to be populated</param>
        /// <param name="dto">the DTO containing values to set to the entity</param>
        /// <exception cref="Exception">Thrown if a property in the DTO does not exist on the entity</exception>
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

        /// <summary>
        /// Converts an entity instance into a DTO of the specified type.
        /// Only properties with matching names are mapped; null values are skipped.
        /// </summary>
        /// <typeparam name="TEntity">The type of the source entity</typeparam>
        /// <typeparam name="TDto">The type of the target Dto </typeparam>
        /// <param name="entity">The entity instance to convert. Returns null if the entity is null.</param>
        /// <returns>A DTO instance of type <typeparamref name="TDto"/> with values copied from the entity. </returns>
        /// <exception cref="Exception">Thrown if a property in the DTO does not exist on the entity. </exception>
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

        /// <summary>
        /// Converts a collection of entity instances into a list of DTOs.
        /// Uses <see cref="ToDto{TEntity,TDto}"/> for each entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the source entities.</typeparam>
        /// <typeparam name="TDto">The type of the target DTOs.</typeparam>
        /// <param name="entities">The collection of entities to convert. Returns an empty list if null.</param>
        /// <returns>A list of DTO instances of type <typeparamref name="TDto"/>.</returns>
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

