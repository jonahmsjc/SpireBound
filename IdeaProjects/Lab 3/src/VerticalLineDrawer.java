import javax.swing.*;
import java.awt.*;
import java.awt.geom.Line2D;
import java.util.Random;

public class VerticalLineDrawer extends JFrame {
    private final DrawingPanel drawingPanel;

    public VerticalLineDrawer() {
        setTitle("Vertical Line Drawer");
        setSize(600, 600);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        setLocationRelativeTo(null);

        drawingPanel = new DrawingPanel();

        JButton redrawButton = new JButton("Redraw");
        redrawButton.addActionListener(_ -> {
            drawingPanel.generateRandomLines();
            drawingPanel.repaint();
        });

        add(drawingPanel, BorderLayout.CENTER);
        add(redrawButton, BorderLayout.SOUTH);
    }

    static class DrawingPanel extends JPanel {
        private final Random rand = new Random();

        private Line2D.Double[] lines;
        private Color[] lineColors;
        public DrawingPanel() {
            generateRandomLines();
        }

        public void generateRandomLines() {
            int NUM_LINES = 10;
            lines = new Line2D.Double[NUM_LINES];
            lineColors = new Color[NUM_LINES];

            int width = getWidth() > 0 ? getWidth() : 600;
            int height = getHeight() > 0 ? getHeight() : 500;

            for (int i = 0; i < NUM_LINES; i++) {
                /* random x, fixed top-to-bottom line */
                int x = rand.nextInt(width);
                int y1 = rand.nextInt(height);

                lines[i] = new Line2D.Double(x, y1, x, height);

                lineColors[i] = new Color(rand.nextInt(256), rand.nextInt(256), rand.nextInt(256));
            }
        }

        @Override
        protected void paintComponent(Graphics g) {
            super.paintComponent(g);

            Graphics2D g2d = (Graphics2D) g;
            g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

            int width = getWidth();
            int height = getHeight();

            // Draw 10x10 grid
            g2d.setColor(Color.LIGHT_GRAY);
            int cellWidth = width / 10;
            int cellHeight = height / 10;

            for (int i = 0; i <= 10; i++) {
                g2d.drawLine(i * cellWidth, 0, i * cellWidth, height);
                g2d.drawLine(0, i * cellHeight, width, i * cellHeight);
            }

            // Draw vertical lines
            g2d.setStroke(new BasicStroke(10f));
            for (int i = 0; i < lines.length; i++) {
                g2d.setColor(lineColors[i]);
                g2d.draw(lines[i]);
            }
        }
    }

    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> new VerticalLineDrawer().setVisible(true));
    }
}
