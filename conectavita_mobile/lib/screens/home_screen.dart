import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import '../models/agendamento.dart';
import '../services/api_service.dart';
import 'login_screen.dart';
import 'agendamento_form_screen.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  List<Agendamento> _agendamentos = [];
  bool _carregando = true;
  String _nome = '';
  String _perfil = '';

  @override
  void initState() {
    super.initState();
    _carregarDados();
  }

  Future<void> _carregarDados() async {
    final userData = await ApiService.getUserData();
    final lista = await ApiService.getAgendamentos();

    if (!mounted) return;

    setState(() {
      _nome = userData['nome'] ?? '';
      _perfil = userData['perfil'] ?? '';
      _agendamentos = lista.map((e) => Agendamento.fromJson(e)).toList();
      _carregando = false;
    });
  }

  Future<void> _logout() async {
    await ApiService.logout();
    if (!mounted) return;
    Navigator.pushReplacement(
      context,
      MaterialPageRoute(builder: (_) => const LoginScreen()),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('ConectaVita'),
        backgroundColor: const Color(0xFF1E3A5F),
        foregroundColor: Colors.white,
        actions: [
          IconButton(icon: const Icon(Icons.logout), onPressed: _logout),
        ],
      ),
      body: _carregando
          ? const Center(child: CircularProgressIndicator())
          : RefreshIndicator(
              onRefresh: _carregarDados,
              child: Column(
                children: [
                  Container(
                    width: double.infinity,
                    padding: const EdgeInsets.all(20),
                    color: const Color(0xFF1E3A5F),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text('Olá, $_nome',
                            style: const TextStyle(
                                color: Colors.white,
                                fontSize: 20,
                                fontWeight: FontWeight.bold)),
                        const SizedBox(height: 4),
                        Text('Perfil: $_perfil',
                            style: const TextStyle(color: Colors.white70)),
                      ],
                    ),
                  ),
                  Expanded(
                    child: _agendamentos.isEmpty
                        ? const Center(
                            child: Text('Nenhum agendamento encontrado.'))
                        : ListView.builder(
                            padding: const EdgeInsets.all(16),
                            itemCount: _agendamentos.length,
                            itemBuilder: (context, index) {
                              final a = _agendamentos[index];
                              return Card(
                                margin: const EdgeInsets.only(bottom: 12),
                                child: ListTile(
                                  leading: const Icon(Icons.calendar_today,
                                      color: Color(0xFF1E3A5F)),
                                  title: Text(a.fisioterapeutaNome ??
                                      'Fisioterapeuta'),
                                  subtitle: Text(
                                    '${DateFormat('dd/MM/yyyy HH:mm').format(a.dataHora)}\nPaciente: ${a.pacienteNome ?? ''}',
                                  ),
                                  trailing: Row(
                                    mainAxisSize: MainAxisSize.min,
                                    children: [
                                      Chip(
                                        label: Text(a.status,
                                            style:
                                                const TextStyle(fontSize: 11)),
                                        backgroundColor: Colors.blue.shade50,
                                      ),
                                      PopupMenuButton<String>(
                                        onSelected: (value) async {
                                          if (value == 'editar') {
                                            final ok = await Navigator.push(
                                              context,
                                              MaterialPageRoute(
                                                builder: (_) =>
                                                    AgendamentoFormScreen(
                                                        agendamento: a),
                                              ),
                                            );
                                            if (ok == true) _carregarDados();
                                          } else if (value == 'excluir') {
                                            final confirmar =
                                                await showDialog<bool>(
                                              context: context,
                                              builder: (ctx) => AlertDialog(
                                                title: const Text(
                                                    'Excluir agendamento'),
                                                content: const Text(
                                                    'Tem certeza que deseja excluir este agendamento?'),
                                                actions: [
                                                  TextButton(
                                                    onPressed: () =>
                                                        Navigator.pop(
                                                            ctx, false),
                                                    child:
                                                        const Text('Cancelar'),
                                                  ),
                                                  TextButton(
                                                    onPressed: () =>
                                                        Navigator.pop(
                                                            ctx, true),
                                                    child: const Text(
                                                      'Excluir',
                                                      style: TextStyle(
                                                          color: Colors.red),
                                                    ),
                                                  ),
                                                ],
                                              ),
                                            );
                                            if (confirmar == true) {
                                              final ok = await ApiService
                                                  .deletarAgendamento(a.id);
                                              if (ok) _carregarDados();
                                            }
                                          }
                                        },
                                        itemBuilder: (context) => [
                                          const PopupMenuItem(
                                            value: 'editar',
                                            child: Row(
                                              children: [
                                                Icon(Icons.edit, size: 18),
                                                SizedBox(width: 8),
                                                Text('Editar'),
                                              ],
                                            ),
                                          ),
                                          const PopupMenuItem(
                                            value: 'excluir',
                                            child: Row(
                                              children: [
                                                Icon(Icons.delete,
                                                    size: 18,
                                                    color: Colors.red),
                                                SizedBox(width: 8),
                                                Text('Excluir',
                                                    style: TextStyle(
                                                        color: Colors.red)),
                                              ],
                                            ),
                                          ),
                                        ],
                                      ),
                                    ],
                                  ),
                                ),
                              );
                            },
                          ),
                  ),
                ],
              ),
            ),
      floatingActionButton: FloatingActionButton.extended(
        onPressed: () async {
          final ok = await Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => const AgendamentoFormScreen()),
          );
          if (ok == true) _carregarDados();
        },
        backgroundColor: const Color(0xFF1E3A5F),
        icon: const Icon(Icons.add),
        label: const Text('Novo Agendamento'),
      ),
    );
  }
}