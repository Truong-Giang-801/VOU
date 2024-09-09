import 'package:flutter/material.dart';
import 'package:vou_web/class/event.dart'; // Import your Event model
import 'package:vou_web/api_handler/event_api_handler.dart'; // Import your API handler

class EventManagerPage extends StatefulWidget {
  final String brandID; // Pass the brandID from the logged-in user
  EventManagerPage({required this.brandID});

  @override
  _EventManagerPageState createState() => _EventManagerPageState();
}

class _EventManagerPageState extends State<EventManagerPage> {
  List<Event> _events = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _fetchEvents();
  }

  // Fetch events by brandID
  Future<void> _fetchEvents() async {
    setState(() {
      _isLoading = true;
    });

    try {
      _events = await getEventsByBrandID(widget.brandID);
    } catch (error) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching events: $error')),
      );
    }

    setState(() {
      _isLoading = false;
    });
  }

  // Add new event
  void _addEvent() {
    showDialog(
      context: context,
      builder: (context) {
        return EventFormDialog(
          onSave: (event) async {
            await addEvent(event);
            _fetchEvents(); // Refresh the list after adding
          },
        );
      },
    );
  }

  // Edit event
  void _editEvent(Event event) {
    showDialog(
      context: context,
      builder: (context) {
        return EventFormDialog(
          event: event,
          onSave: (updatedEvent) async {
            await updateEvent(updatedEvent);
            _fetchEvents(); // Refresh the list after updating
          },
        );
      },
    );
  }

  // Delete event
  Future<void> _deleteEvent(int id) async {
    await deleteEvent(id);
    _fetchEvents(); // Refresh the list after deleting
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
  final Function(Event) onSave;

  EventFormDialog({this.event, required this.onSave});

  @override
  _EventFormDialogState createState() => _EventFormDialogState();
}

class _EventFormDialogState extends State<EventFormDialog> {
  late TextEditingController _nameController;
  late TextEditingController _imgController;
  late TextEditingController _voucherController;

  @override
  void initState() {
    super.initState();
    _nameController = TextEditingController(text: widget.event?.name ?? '');
    _imgController = TextEditingController(text: widget.event?.img ?? '');
    _voucherController = TextEditingController(text: widget.event?.numberOfVoucher.toString() ?? '0');
  }

  void _saveEvent() {
    final name = _nameController.text;
    final img = _imgController.text;
    final vouchers = int.tryParse(_voucherController.text) ?? 0;

    final newEvent = Event(
      id: widget.event?.id ?? 0,
      brandId: widget.event?.brandId ?? 0, // Pass brand ID
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
