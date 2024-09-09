import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:vou_web/class/game.dart';

class GameApiHandler {
  static const String _baseUrl = 'https://localhost:7004/api/Game';

  // Fetch all games
  static Future<List<Game>> fetchGames() async {
    try {
      final response = await http.get(Uri.parse(_baseUrl));
      if (response.statusCode == 200) {
        final List<dynamic> data = json.decode(response.body)['result'];
        return data.map((json) => Game.fromJson(json)).toList();
      } else {
        throw Exception('Failed to load games');
      }
    } catch (e) {
      throw Exception('Error fetching games: $e');
    }
  }

  // Fetch a single game by ID
  static Future<Game> fetchGame(int id) async {
    try {
      final response = await http.get(Uri.parse('$_baseUrl/$id'));
      if (response.statusCode == 200) {
        return Game.fromJson(json.decode(response.body)['result']);
      } else {
        throw Exception('Failed to load game');
      }
    } catch (e) {
      throw Exception('Error fetching game: $e');
    }
  }

  // Add a new game
  static Future<Game> addGame(Game game) async {
    try {
      final response = await http.post(
        Uri.parse(_baseUrl),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(game.toJson()),
      );
      if (response.statusCode == 200) {
        return Game.fromJson(json.decode(response.body)['result']);
      } else {
        throw Exception('Failed to add game');
      }
    } catch (e) {
      throw Exception('Error adding game: $e');
    }
  }

  // Update an existing game
  static Future<Game> updateGame(Game game) async {
    try {
      final response = await http.put(
        Uri.parse('$_baseUrl/${game.id}'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(game.toJson()),
      );
      if (response.statusCode == 200) {
        return Game.fromJson(json.decode(response.body)['result']);
      } else {
        throw Exception('Failed to update game');
      }
    } catch (e) {
      throw Exception('Error updating game: $e');
    }
  }

  // Delete a game
  static Future<void> deleteGame(int id) async {
    try {
      final response = await http.delete(Uri.parse('$_baseUrl/$id'));
      if (response.statusCode != 200) {
        throw Exception('Failed to delete game');
      }
    } catch (e) {
      throw Exception('Error deleting game: $e');
    }
  }
}
