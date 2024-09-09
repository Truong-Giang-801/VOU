import 'package:flutter/material.dart';
import 'package:vou_web/class/game.dart';
import 'package:vou_web/api_handler/game_api_handler.dart'; // Import the API handler
import 'package:vou_web/pages/edit_game_page.dart'; // Import the EditGamePage

class ManageGamePage extends StatefulWidget {
  @override
  _ManageGamePageState createState() => _ManageGamePageState();
}

class _ManageGamePageState extends State<ManageGamePage> {
  List<Game> _games = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _fetchGames();
  }

  Future<void> _fetchGames() async {
    try {
      final games = await GameApiHandler.fetchGames();
      setState(() {
        _games = games;
        _isLoading = false;
      });
    } catch (e) {
      // Handle error
      setState(() {
        _isLoading = false;
      });
      // Optionally show an error message
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching games: $e')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Manage Games'),
        backgroundColor: Colors.cyan,
      ),
      body: _isLoading
          ? Center(child: CircularProgressIndicator())
          : ListView.builder(
              itemCount: _games.length,
              itemBuilder: (context, index) {
                final game = _games[index];
                return ListTile(
                  leading: Image.network(game.img, width: 50, height: 50, fit: BoxFit.cover),
                  title: Text(game.name),
                  subtitle: Text(game.instruction),
                  trailing: ElevatedButton(
                    onPressed: () {
                      Navigator.push(
                        context,
                        MaterialPageRoute(
                          builder: (context) => EditGamePage(game: game),
                        ),
                      ).then((_) {
                        // Refresh the game list after returning from the EditGamePage
                        _fetchGames();
                      });
                    },
                    child: Text('Edit'),
                  ),
                );
              },
            ),
    );
  }
}
