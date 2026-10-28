class Agendamento {
  final int id;
  final int pacienteId;
  final String? pacienteNome;
  final int fisioterapeutaId;
  final String? fisioterapeutaNome;
  final DateTime dataHora;
  final String status;
  final String? observacoes;

  Agendamento({
    required this.id,
    required this.pacienteId,
    this.pacienteNome,
    required this.fisioterapeutaId,
    this.fisioterapeutaNome,
    required this.dataHora,
    required this.status,
    this.observacoes,
  });

  factory Agendamento.fromJson(Map<String, dynamic> json) {
    return Agendamento(
      id: json['id'] ?? 0,
      pacienteId: json['pacienteId'] ?? 0,
      pacienteNome: json['pacienteNome'],
      fisioterapeutaId: json['fisioterapeutaId'] ?? 0,
      fisioterapeutaNome: json['fisioterapeutaNome'],
      dataHora: DateTime.tryParse(json['dataHora'] ?? '') ?? DateTime.now(),
      status: json['status'] ?? 'Agendado',
      observacoes: json['observacoes'],
    );
  }
}