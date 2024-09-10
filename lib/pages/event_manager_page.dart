import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:vou_web/api_handler/user_api_handler.dart'; // Adjust the import path as necessary
import 'package:vou_web/class/event.dart'; // Import your Event model
import 'package:vou_web/api_handler/event_api_handler.dart'; // Adjust the import path as necessary

class EventManagerPage extends StatefulWidget {
  final String userId;

  EventManagerPage({required this.userId});

  @override
  _EventManagerPageState createState() => _EventManagerPageState();
}

class _EventManagerPageState extends State<EventManagerPage> {
  List<Event> _events = [];
  bool _isLoading = true;
  late int _brandID;

  @override
  void initState() {
    super.initState();
    _fetchEvents();
  }

  Future<void> _fetchEvents() async {
    setState(() {
      _isLoading = true;
    });

    try {
      _brandID = await getBrandIdByUserId(widget.userId);
      print('Brand ID: $_brandID');

      if (_brandID != 0) {
        final response = await getEventsByBrandID(_brandID);

        if (response.statusCode == 200) {
          final List<dynamic> responseData = json.decode(response.body)['result'];
          setState(() {
            _events = responseData.map((json) => Event.fromJson(json)).toList();
          });
        } else {
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Error fetching events: ${response.reasonPhrase}')),
          );
          setState(() {
            _events = [];
          });
        }
      } else {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Brand ID not found for user.')),
        );
        setState(() {
          _events = [];
        });
      }
    } catch (error) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching events: $error')),
      );
      setState(() {
        _events = [];
      });
    }

    setState(() {
      _isLoading = false;
    });
  }

  void _addEvent() {
    showDialog(
      context: context,
      builder: (context) {
        return EventFormDialog(
          brandId: _brandID,
          onSave: (event) async {
            final success = await addEvent(event);
            if (success) {
              _fetchEvents(); // Refresh the list after adding
            } else {
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(content: Text('Failed to add event')),
              );
            }
          },
        );
      },
    );
  }

  void _editEvent(Event event) {
    showDialog(
      context: context,
      builder: (context) {
        return EventFormDialog(
          brandId: event.brandId,
          event: event,
          onSave: (updatedEvent) async {
            final success = await updateEvent(updatedEvent);
            if (success) {
              _fetchEvents(); // Refresh the list after updating
            } else {
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(content: Text('Failed to update event')),
              );
            }
          },
        );
      },
    );
  }

  Future<void> _deleteEvent(int id) async {
    final success = await deleteEvent(id);
    if (success) {
      _fetchEvents(); // Refresh the list after deleting
    } else {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Failed to delete event')),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Event Manager'),
        actions: [
          IconButton(
            icon: Icon(Icons.add),
            onPressed: _addEvent, // Add event button
          ),
        ],
      ),
      body: _isLoading
          ? Center(child: CircularProgressIndicator())
          : _events.isEmpty
              ? Center(child: Text('No events available'))
              : ListView.builder(
                  itemCount: _events.length,
                  itemBuilder: (context, index) {
                    final event = _events[index];
                    return ListTile(
                      leading: Image.network(event.img), // Display event image
                      title: Text(event.name),
                      subtitle: Text('Vouchers: ${event.numberOfVoucher}'),
                      trailing: Row(
                        mainAxisSize: MainAxisSize.min,
                        children: [
                          IconButton(
                            icon: Icon(Icons.edit),
                            onPressed: () => _editEvent(event), // Edit event
                          ),
                          IconButton(
                            icon: Icon(Icons.delete),
                            onPressed: () async {
                              await _deleteEvent(event.id); // Delete event
                            },
                          ),
                        ],
                      ),
                    );
                  },
                ),
    );
  }
}

class EventFormDialog extends StatefulWidget {
  final Event? event;
  final int brandId;
  final Function(Event) onSave;

  EventFormDialog({this.event, required this.brandId, required this.onSave});

  @override
  _EventFormDialogState createState() => _EventFormDialogState();
}

class _EventFormDialogState extends State<EventFormDialog> {
  late TextEditingController _nameController;
  late TextEditingController _imgController;
  late TextEditingController _voucherController;
  late TextEditingController _gameIdController;

  @override
  void initState() {
    super.initState();
    _nameController = TextEditingController(text: widget.event?.name ?? '');
    _imgController = TextEditingController(text: widget.event?.img ?? '');
    _voucherController = TextEditingController(text: widget.event?.numberOfVoucher.toString() ?? '0');
    _gameIdController = TextEditingController(text: widget.event?.gameId.toString() ?? '0');
  }

  void _saveEvent() {
    final name = _nameController.text;
    final img = _imgController.text;
    final vouchers = int.tryParse(_voucherController.text) ?? 0;
    final gameId = int.tryParse(_gameIdController.text) ?? 0;

    final newEvent = Event(
      id: widget.event?.id ?? 0,
      brandId: widget.brandId, // Use brandId passed to the dialog
      gameId: gameId,
      name: name,
      img: img,
      numberOfVoucher: vouchers,
      dateCreated: widget.event?.dateCreated ?? DateTime.now(),
      dateUpdated: DateTime.now(),
    );

    widget.onSave(newEvent);
    Navigator.of(context).pop();
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: Text(widget.event == null ? 'Add Event' : 'Edit Event'),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          TextField(
            controller: _nameController,
            decoration: InputDecoration(labelText: 'Event Name'),
          ),
          TextField(
            controller: _imgController,
            decoration: InputDecoration(labelText: 'Image URL'),
          ),
          TextField(
            controller: _voucherController,
            decoration: InputDecoration(labelText: 'Number of Vouchers'),
            keyboardType: TextInputType.number,
          ),
          TextField(
            controller: _gameIdController,
            decoration: InputDecoration(labelText: 'Game ID'),
            keyboardType: TextInputType.number,
          ),
        ],
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.of(context).pop(),
          child: Text('Cancel'),
        ),
        ElevatedButton(
          onPressed: _saveEvent,
          child: Text('Save'),
        ),
      ],
    );
  }
}
