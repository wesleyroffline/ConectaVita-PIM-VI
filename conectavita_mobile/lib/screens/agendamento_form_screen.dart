import 'package:flutter/material.dart';
import '../models/agendamento.dart';
import '../services/api_service.dart';

class AgendamentoFormScreen extends StatefulWidget {
  final Agendamento? agendamento; // Se null = criar; se preenchido = editar

  const AgendamentoFormScreen({super.key, this.agendamento});

  @override
  State<AgendamentoFormScreen> createState() => _AgendamentoFormScreenState();
}

class _AgendamentoFormScreenState extends State<AgendamentoFormScreen> {
  List<dynamic> _pacientes = [];
  List<dynamic> _fisios = [];
  int? _pacienteId;
  int? _fisioId;
  late DateTime _dataHora;
  final _obsController = TextEditingController();
  bool _carregando = true;
  bool _salvando = false;

  bool get _editando => widget.agendamento != null;

  @override
  void initState() {
    super.initState();

    if (_editando) {
      _pacienteId = widget.agendamento!.pacienteId;
      _fisioId = widget.agendamento!.fisioterapeutaId;
      _dataHora = widget.agendamento!.dataHora;
      _obsController.text = widget.agendamento!.observacoes ?? '';
    } else {
      _dataHora = DateTime.now().add(const Duration(days: 1));
    }

    _carregarListas();
  }

  @override
  void dispose() {
    _obsController.dispose();
    super.dispose();
  }

  Future<void> _carregarListas() async {
    final p = await ApiService.getPacientes();
    final f = await ApiService.getFisioterapeutas();
    if (!mounted) return;
    setState(() {
      _pacientes = p;
      _fisios = f;
      _carregando = false;
    });
  }

  Future<void> _salvar() async {
    if (_pacienteId == null || _fisioId == null) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('Selecione paciente e fisioterapeuta')),
      );
      return;
    }

    setState(() => _salvando = true);

    bool ok;
    if (_editando) {
      ok = await ApiService.editarAgendamento(
        widget.agendamento!.id,
        _pacienteId!,
        _fisioId!,
        _dataHora,
        _obsController.text,
      );
    } else {
      ok = await ApiService.criarAgendamento(
        _pacienteId!,
        _fisioId!,
        _dataHora,
        _obsController.text,
      );
    }

    if (!mounted) return;

    if (ok) {
      Navigator.pop(context, true); // devolve true = sucesso
    } else {
      setState(() => _salvando = false);
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text(_editando
            ? 'Erro ao editar agendamento'
            : 'Erro ao criar agendamento')),
      );
    }
  }

  Future<void> _escolherDataHora() async {
    final data = await showDatePicker(
      context: context,
      initialDate: _dataHora,
      firstDate: DateTime.now().subtract(const Duration(days: 365)),
      lastDate: DateTime.now().add(const Duration(days: 365)),
    );
    if (data == null) return;

    if (!mounted) return;
    final hora = await showTimePicker(
      context: context,
      initialTime: TimeOfDay.fromDateTime(_dataHora),
    );
    if (hora == null) return;

    setState(() {
      _dataHora =
          DateTime(data.year, data.month, data.day, hora.hour, hora.minute);
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(_editando ? 'Editar Agendamento' : 'Novo Agendamento'),
        backgroundColor: const Color(0xFF1E3A5F),
        foregroundColor: Colors.white,
      ),
      body: _carregando
          ? const Center(child: CircularProgressIndicator())
          : SingleChildScrollView(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  DropdownButtonFormField<int>(
                    decoration: const InputDecoration(
                        labelText: 'Paciente', border: OutlineInputBorder()),
                    value: _pacienteId,
                    items: _pacientes.map<DropdownMenuItem<int>>((p) {
                      return DropdownMenuItem(
                          value: p['id'], child: Text(p['nome']));
                    }).toList(),
                    onChanged: (v) => setState(() => _pacienteId = v),
                  ),
                  const SizedBox(height: 16),
                  DropdownButtonFormField<int>(
                    decoration: const InputDecoration(
                        labelText: 'Fisioterapeuta',
                        border: OutlineInputBorder()),
                    value: _fisioId,
                    items: _fisios.map<DropdownMenuItem<int>>((f) {
                      return DropdownMenuItem(
                          value: f['id'], child: Text(f['nome']));
                    }).toList(),
                    onChanged: (v) => setState(() => _fisioId = v),
                  ),
                  const SizedBox(height: 16),
                  OutlinedButton.icon(
                    onPressed: _escolherDataHora,
                    icon: const Icon(Icons.calendar_today),
                    label: Text(
                        'Data/Hora: ${_dataHora.day}/${_dataHora.month}/${_dataHora.year} ${_dataHora.hour}:${_dataHora.minute.toString().padLeft(2, '0')}'),
                  ),
                  const SizedBox(height: 16),
                  TextField(
                    controller: _obsController,
                    decoration: const InputDecoration(
                        labelText: 'Observações', border: OutlineInputBorder()),
                    maxLines: 3,
                  ),
                  const SizedBox(height: 24),
                  SizedBox(
                    height: 50,
                    child: ElevatedButton(
                      onPressed: _salvando ? null : _salvar,
                      style: ElevatedButton.styleFrom(
                        backgroundColor: const Color(0xFF1E3A5F),
                        foregroundColor: Colors.white,
                      ),
                      child: _salvando
                          ? const CircularProgressIndicator(color: Colors.white)
                          : Text(_editando ? 'Salvar Alterações' : 'Salvar'),
                    ),
                  ),
                ],
              ),
            ),
    );
  }
}