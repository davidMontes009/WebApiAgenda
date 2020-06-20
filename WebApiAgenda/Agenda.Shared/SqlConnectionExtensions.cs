using Agenda.Shared.Enums;
using System.Data;
using System.Data.SqlClient;

namespace Agenda.Shared
{
    /// <summary>
    /// Clase para mapear las sentencia SQL
    /// </summary>
    public static class SqlConnectionExtensions
    {
        public static SqlCommand CreateSpCommand(this SqlConnection connection, string spName, OperationBD? OperacionBD = null)
        {
            var command = new SqlCommand(spName, connection);
            command.CommandType = CommandType.StoredProcedure;
            if (OperacionBD.HasValue)
            {
                command.AddSqlParameter("@OPERATION", OperacionBD.Value);
            }
            return command;
        }
    }


    public static class SqlCommandExtensions
    {

        /// <summary>
        /// Metodo que agrega el parametro con su tipo 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameterName"> Nombre del parameteo </param>
        /// <param name="type">tipo parameteo </param>
        /// <param name="value"> valor </param>
        public static void AddSqlParameter(this SqlCommand command, string parameterName, SqlDbType type, object value)
         => command.Parameters.Add(CreateParameter(parameterName, type, value));

        /// <summary>
        /// Metodo que agrega el parametro 
        /// </summary>
        /// <param name="parameterName">Nombre del parameteo </param>
        /// <param name="value"> valor del parametro  </param>
        public static void AddSqlParameter(this SqlCommand command, string parameterName, object value)
         => command.Parameters.Add(CreateParameter(parameterName, value));

        public static void AddSqlParameter(this SqlCommand command, SqlParameter sqlParameter)
            => command.Parameters.Add(sqlParameter);

        /// <summary>
        /// Agrega parametro de salida a la sentencia 
        /// </summary>
        /// <param name="command"> SqlCommand </param>
        /// <param name="_paremeterName"> nombre del parametro </param>
        /// <param name="type">tipo del parametro </param>
        public static void AddOutputSqlParameter(this SqlCommand command, string _paremeterName, SqlDbType type)
            => command.Parameters.Add(CreateOutputParameter(_paremeterName, type));

        public static SqlParameter CreateParameter(string parameterName, object value)
             => new SqlParameter() { ParameterName = parameterName, Value = value };

        public static SqlParameter CreateParameter(string parameterName, SqlDbType type, object value)
             => new SqlParameter() { ParameterName = parameterName, SqlDbType = type, Value = value };

        public static SqlParameter CreateOutputParameter(string parameterName, SqlDbType type)
            => new SqlParameter() { ParameterName = parameterName, SqlDbType = type, Direction = ParameterDirection.Output };
    }
}
