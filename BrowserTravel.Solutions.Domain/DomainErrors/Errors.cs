namespace BrowserTravel.Solutions.Domain.DomainErrors;

public static partial class Errors
{
    public static class GenericError
    {
        public static string WithBadCharacterNumber(string attribute, int quantityRequired, int currentQuantity) => $"{attribute} debe ser mayor o igual que {quantityRequired} caracteres. Ingresó {currentQuantity} caracter(es).";
        public static string IsNullOrWhiteSpace(string attribute) => $"{attribute} no puede estar vacío o contener solo espacios en blanco";
        public static string IsNullValue(string attribute) => $"{attribute} tiene un valor nulo";
        public static string NotFound(string attribute) => $"{attribute} no encontrado";
        public static string NotFoundProperty(string propertyName, string entity) => $"La propiedad {propertyName} no existe en la clase {entity}";
    }

    public static class PersonError
    {
        public static string WithBadCharacterNumber(string attribute, int quantityRequired, int currentQuantity) => $"{attribute} debe ser mayor o igual que {quantityRequired} caracteres. Ingresó {currentQuantity} caracter(es).";

    }

    public static class RoleError
    {
        public static string RoleAlreadyExists() => $" El rol ya existe.";


    }

    public static class UserLoginError
    {
        public static string ValidateExpires() => $" Validar Expires en la sesión Jwt de la configuración de la aplicación.";


    }
}
