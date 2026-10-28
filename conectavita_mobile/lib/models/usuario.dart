class Usuario {
  final String nome;
  final String email;
  final String perfil;
  final String token;

  Usuario({
    required this.nome,
    required this.email,
    required this.perfil,
    required this.token,
  });

  factory Usuario.fromJson(Map<String, dynamic> json) {
    return Usuario(
      nome: json['nome'] ?? '',
      email: json['email'] ?? '',
      perfil: json['perfil'] ?? 'Paciente',
      token: json['token'] ?? '',
    );
  }
}