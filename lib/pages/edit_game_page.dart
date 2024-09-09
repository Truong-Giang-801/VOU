import 'package:flutter/material.dart';
import 'package:vou_web/class/game.dart';
import 'package:vou_web/api_handler/game_api_handler.dart'; // Import the API handler

class EditGamePage extends StatefulWidget {
  final Game game;

  EditGamePage({required this.game});

  @override
  _EditGamePageState createState() => _EditGamePageState();
}

class _EditGamePageState extends State<EditGamePage> {
  final _formKey = GlobalKey<FormState>();
  late Game _game;

  @override
  void initState() {
    super.initState();
    _game = widget.game;
  }

  void _save() async {
    if (_formKey.currentState?.validate() ?? false) {
      try {
        final updatedGame = await GameApiHandler.updateGame(_game);
        Navigator.pop(context, updatedGame);
      } catch (e) {
        // Handle error
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Error updating game: $e')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Edit Game'),
        backgroundColor: Colors.cyan,
      ),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Form(
          key: _formKey,
          child: Column(
            children: [
              TextFormField(
                initialValue: _game.name,
                decoration: InputDecoration(labelText: 'Game Name'),
                onChanged: (value) => setState(() => _game.name = value),
                validator: (value) => value?.isEmpty ?? true ? 'Please enter a name' : null,
              ),
              TextFormField(
                initialValue: _game.img,
                decoration: InputDecoration(labelText: 'Image URL'),
                onChanged: (value) => setState(() => _game.img = value),
              ),
              TextFormField(
                initialValue: _game.instruction,
                decoration: InputDecoration(labelText: 'Instruction'),
                onChanged: (value) => setState(() => _game.instruction = value),
              ),
              SwitchListTile(
                title: Text('Allow Trade'),
                value: _game.allowTrade,
                onChanged: (value) => setState(() => _game.allowTrade = value),
              ),
              SizedBox(height: 20),
              ElevatedButton(
                onPressed: _save,
                child: Text('Save'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
