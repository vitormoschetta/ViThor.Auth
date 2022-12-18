namespace ViThor.Auth.Settings
{
    public class JwtConfig
    {
        /// <summary>
        /// Chave Secreta para geração do Token. 
        /// <para></para>
        /// Secret key for generating the Token.
        /// </summary>
        public string Secret { get; set; } = string.Empty;

        /// <summary>
        /// Emissor do Token.
        /// <para></para>
        /// Issuer of the Token.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Público alvo do Token.
        /// <para></para> 
        /// Audience of the Token.
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de Tempo de expiração do Token.
        /// <para></para>
        /// Type of Token expiration time.
        /// </summary>
        public string ExpirationType { get; set; } = string.Empty;

        /// <summary>
        /// Tempo de expiração do Token baseado no tipo de tempo.
        /// <para></para>
        /// Token expiration time based on the type of time.
        /// </summary>
        public int Expiration { get; set; }

        /// <summary>
        /// Verifica se é necessário validar a chave de assinatura do Token.
        /// <para></para>
        /// Checks if it is necessary to validate the signature key of the Token.
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; }

        /// <summary>
        /// Verifica se é necessário validar a data de expiração do Token.
        /// <para></para>
        /// Checks if it is necessary to validate the expiration date of the Token.
        /// </summary>
        public bool ValidateLifetime { get; set; }

        /// <summary>
        /// Verifica se é necessário validar o emissor do Token.
        /// <para></para>
        /// Checks if it is necessary to validate the issuer of the Token.
        /// </summary>
        public bool ValidateIssuer { get; set; }

        /// <summary>
        /// Verifica se é necessário validar o público alvo do Token.
        /// <para></para>
        /// Checks if it is necessary to validate the target audience of the Token.
        /// </summary>
        public bool ValidateAudience { get; set; }

        /// <summary>
        /// Verifica se é necessário validar se a conexão é HTTPS.
        /// <para></para>
        /// Checks if it is necessary to validate if the connection is HTTPS.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// Verifica se é necessário salvar o Token no contexto da requisição atual.
        /// <para></para>
        /// Checks if it is necessary to save the Token in the context of the current request.
        /// </summary>
        public bool SaveToken { get; set; }
    }
}