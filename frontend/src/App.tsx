import { useEffect, useState, useCallback } from 'react';
import {
  Container,
  Typography,
  Paper,
  TextField,
  Select,
  MenuItem,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  FormControl,
  InputLabel,
  Box,
} from '@mui/material';
import { Book } from './types/book';
import { searchBooks } from './services/api';

function App() {
  const [searchBy, setSearchBy] = useState('');
  const [searchValue, setSearchValue] = useState('');
  const [books, setBooks] = useState<Book[]>([]);

  const fetchBooks = useCallback(async () => {
    try {
      const params: Partial<Record<string, string>> = {};
      if (searchBy && searchValue) {
        params[searchBy] = searchValue;
      }
      const results = await searchBooks(params);
      setBooks(results);
    } catch (error) {
      console.error('Error fetching books:', error);
    }
  }, [searchBy, searchValue]);

  useEffect(() => {
    void fetchBooks();
  }, [fetchBooks]);

  const handleSearch = () => {
    fetchBooks();
  };

  return (
    <Box
      sx={{
        width: '100vw',
        height: '100vh',
        margin: 'auto',
        p: 2,
        boxSizing: 'border-box',
      }}
    >
      <Box
        sx={{
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          mb: 2,
        }}
      >
        <Container
          maxWidth="xl"
          sx={{
            py: 4,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center', // Center the content horizontally
          }}
        >
          <Typography
            variant="h3"
            component="h1"
            gutterBottom
            align="center"
            color="primary"
          >
            Royal Library
          </Typography>

          <Paper sx={{ p: 3, mb: 4, width: '100%', maxWidth: 800 }}>
            <Box sx={{ display: 'flex', gap: 2, alignItems: 'flex-end' }}>
              <FormControl sx={{ minWidth: 200 }}>
                <InputLabel id="search-by-label">Search By</InputLabel>
                <Select
                  labelId="search-by-label"
                  value={searchBy}
                  label="Search By"
                  onChange={(e) => setSearchBy(e.target.value)}
                >
                  <MenuItem value="title">Book Title</MenuItem>
                  <MenuItem value="author">Author</MenuItem>
                  <MenuItem value="isbn">ISBN</MenuItem>
                  <MenuItem value="publisher">Publisher</MenuItem>
                </Select>
              </FormControl>

              <TextField
                label="Search Value"
                value={searchValue}
                onChange={(e) => setSearchValue(e.target.value)}
                sx={{ flexGrow: 1 }}
              />

              <Button
                variant="contained"
                onClick={handleSearch}
                sx={{ height: 56 }}
              >
                Search
              </Button>
            </Box>
          </Paper>

          <TableContainer
            component={Paper}
            sx={{ width: '100%', maxWidth: 800 }}
          >
            <Typography variant="h6" sx={{ p: 2 }}>
              Search Results:
            </Typography>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Book Title</TableCell>
                  <TableCell>Authors</TableCell>
                  <TableCell>Type</TableCell>
                  <TableCell>ISBN</TableCell>
                  <TableCell>Category</TableCell>
                  <TableCell>Available Copies</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {books.map((book, index) => (
                  <TableRow key={index}>
                    <TableCell>{book.title}</TableCell>
                    <TableCell>
                      {book.firstName} {book.lastName}
                    </TableCell>
                    <TableCell>{book.type}</TableCell>
                    <TableCell>{book.isbn}</TableCell>
                    <TableCell>{book.category}</TableCell>
                    <TableCell>
                      {book.copiesInUse} / {book.totalCopies}{' '}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Container>
      </Box>
    </Box>
  );
}

export default App;
